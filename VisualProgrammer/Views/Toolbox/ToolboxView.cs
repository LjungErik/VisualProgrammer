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
using VisualProgrammer.Controls;
using VisualProgrammer.Utilities;
using VisualProgrammer.Views.Restructure.Designer.Events;

namespace VisualProgrammer.Views.Toolbox
{
    public class ToolboxView : Control, IDropView
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

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(ToolboxView),
                new FrameworkPropertyMetadata(false));

        public static readonly RoutedEvent DraggedOverEvent =
            EventManager.RegisterRoutedEvent("DraggedOver", RoutingStrategy.Bubble, typeof(DragDropEventHandler), typeof(ToolboxView));

        public static readonly RoutedEvent ToolboxItemDropCanceledEvent =
            EventManager.RegisterRoutedEvent("ToolboxItemDropCancel", RoutingStrategy.Bubble, typeof(ToolboxItemDropCanceledEventHandler), typeof(ToolboxView));
       
        #endregion Dependency Property/Event Definitions

        public ToolboxView()
        {
            this.Background = Brushes.White;

            this.Tools = new ImpObservableCollection<object>();

            AddHandler(ToolboxItem.ToolboxItemDropCanceledEvent, new ToolboxItemDropCanceledEventHandler(ToolboxItem_DropCanceled));
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

        public bool IsExpanded
        {
            get
            {
                return (bool)GetValue(IsExpandedProperty);
            }
            set
            {
                SetValue(IsExpandedProperty, value);
            }
        }

        public event RoutedEventHandler DraggedOver
        {
            add { AddHandler(DraggedOverEvent, value); }
            remove { RemoveHandler(DraggedOverEvent, value); }
        }

        public event RoutedEventHandler ToolboxItemDropCanceled
        {
            add { AddHandler(ToolboxItemDropCanceledEvent, value); }
            remove { RemoveHandler(ToolboxItemDropCanceledEvent, value); }
        }

        public new void DragOver(IDraggable dragged)
        {
            RaiseEvent(new DragDropEventArgs(DraggedOverEvent, this, dragged));
        }

        public void Dropped(IDraggable dragged) { }

        #region Private Methods

        static ToolboxView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolboxView), new FrameworkPropertyMetadata(typeof(ToolboxView)));
        }

        private void ToolboxItem_DropCanceled(object sender, ToolboxItemEventArgs e)
        {
            e.Handled = true;

            RaiseEvent(new ToolboxItemEventArgs(ToolboxItemDropCanceledEvent, this, e.Item));
        }

        /// <summary>
        /// Event raised when a new collection has been assigned to the 'ToolsSource' property.
        /// </summary>
        private static void ToolsSource_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           ToolboxView c = (ToolboxView)d;

            c.Tools.Clear();

            if (e.NewValue != null)
            {
                var enumerable = e.NewValue as IEnumerable;
                if (enumerable != null)
                {
                    foreach (object obj in enumerable)
                    {
                        c.Tools.Add(obj);
                    }
                }
            }
        }

        #endregion Private Methods

        
    }
}
