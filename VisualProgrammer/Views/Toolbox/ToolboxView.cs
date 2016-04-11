using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VisualProgrammer.Utilities;

namespace VisualProgrammer.Views.Toolbox
{
    public class ToolboxView : Control
    {
        #region Dependency Property/Event Definitions
        
        private static readonly DependencyPropertyKey ToolsPropertyKey =
            DependencyProperty.RegisterReadOnly("Tools", typeof(ImpObservableCollection<object>), typeof(ToolboxView),
                new FrameworkPropertyMetadata());
        public static readonly DependencyProperty ToolsProperty = ToolsPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ToolsSourceProperty =
            DependencyProperty.Register("ToolsSource", typeof(IEnumerable), typeof(ToolboxView),
                    new FrameworkPropertyMetadata(ToolsSource_PropertyChanged));

        public static readonly DependencyProperty IsNodeDraggedPropertry =
            DependencyProperty.Register("IsNodeDragged", typeof(bool), typeof(ToolboxView));

        public static readonly DependencyProperty IsNodeDraggedOverPropertry =
            DependencyProperty.Register("IsNodeDraggedOver", typeof(bool), typeof(ToolboxView));

        public static readonly RoutedEvent ToolboxItemDragStartedEvent =
            EventManager.RegisterRoutedEvent("ToolboxItemDragStarted", RoutingStrategy.Bubble, typeof(ToolboxItemDragStartedEventHandler), typeof(ToolboxView));

        public static readonly RoutedEvent ToolboxItemDroppedEvent =
            EventManager.RegisterRoutedEvent("ToolboxItemDrop", RoutingStrategy.Bubble, typeof(ToolboxItemDroppedEventHandler), typeof(ToolboxView));
       
        #endregion Dependency Property/Event Definitions

        public ToolboxView()
        {
            this.Background = Brushes.White;

            this.Tools = new ImpObservableCollection<object>();

            //
            // Add handlers for click event on toolbox items
            //
            AddHandler(ToolboxItem.ToolboxItemDragStartedEvent, new ToolboxItemDragStartedEventHandler(ToolboxItem_Clicked));
        }

        /// <summary>
        /// Collection of tools in the toolbox.
        /// </summary>
        public ImpObservableCollection<object> Tools
        {
            get
            {
                return (ImpObservableCollection<object>)GetValue(ToolsProperty);
            }
            private set
            {
                SetValue(ToolsPropertyKey, value);
            }
        }

        /// <summary>
        /// A reference to the collection that is the source used to populate 'Tools'.
        /// Used in the same way as 'ItemsSource' in 'ItemsControl'.
        /// </summary>
        public IEnumerable ToolsSource
        {
            get
            {
                return (IEnumerable)GetValue(ToolsSourceProperty);
            }
            set
            {
                SetValue(ToolsSourceProperty, value);
            }
        }

        /// <summary>
        /// Tells if a node is currently being dragged and allow visible 
        /// delete to be shown in toolbox
        /// </summary>
        public bool IsNodeDragged
        {
            get 
            { 
                return (bool)GetValue(IsNodeDraggedPropertry); 
            }
            set
            {
                SetValue(IsNodeDraggedPropertry, value);
            }
        }

        /// <summary>
        /// Tells if a node is currently being dragged over the
        /// toolbox, show correct toolboxIcon
        /// </summary>
        public bool IsNodeDraggedOver
        {
            get
            {
                return (bool)GetValue(IsNodeDraggedOverPropertry);
            }
            set
            {
                SetValue(IsNodeDraggedOverPropertry, value);
            }
        }

        public event ToolboxItemDragStartedEventHandler ToolboxItemDragStarted
        {
            add { AddHandler(ToolboxItemDragStartedEvent, value); }
            remove { RemoveHandler(ToolboxItemDragStartedEvent, value); }
        }

        public event ToolboxItemDroppedEventHandler ToolboxItemDropped
        {
            add { AddHandler(ToolboxItemDroppedEvent, value); }
            remove { RemoveHandler(ToolboxItemDroppedEvent, value); }
        }

        #region Private Methods

        static ToolboxView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolboxView), new FrameworkPropertyMetadata(typeof(ToolboxView)));
        }

        private void ToolboxItem_Clicked(object sender, ToolboxItemEventArgs e)
        {
            e.Handled = true;

            var eventArgs = new ToolboxItemEventArgs(ToolboxItemDragStartedEvent, this, e.Item);
            RaiseEvent(eventArgs);
        }

        /// <summary>
        /// Event raised when a new collection has been assigned to the 'ToolsSource' property.
        /// </summary>
        private static void ToolsSource_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           ToolboxView c = (ToolboxView)d;

            //
            // Clear 'Tools'.
            //
            c.Tools.Clear();

            if (e.NewValue != null)
            {
                var enumerable = e.NewValue as IEnumerable;
                if (enumerable != null)
                {
                    //
                    // Populate 'Tools' from 'ToolsSource'.
                    //
                    foreach (object obj in enumerable)
                    {
                        c.Tools.Add(obj);
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if(e.ChangedButton == MouseButton.Left)
            {
                //Left Mousebutton was released (signal that dragged item should be dropped)
                RaiseEvent(new ToolboxItemEventArgs(ToolboxItemDroppedEvent, this));
            }
        }

        #endregion Private Methods
    }
}
