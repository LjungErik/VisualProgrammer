using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VisualProgrammer.Views.Toolbox
{
    public class ToolboxItem : ContentControl
    {
        #region Dependency Property/Event Definitions

        internal static readonly RoutedEvent ToolboxItemDragStartedEvent =
            EventManager.RegisterRoutedEvent("ToolboxItemDragStarted", RoutingStrategy.Bubble, typeof(ToolboxItemDragStartedEventHandler), typeof(ToolboxItem));

        #endregion Dependency Property/Event Definitions

        static ToolboxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolboxItem), new FrameworkPropertyMetadata(typeof(ToolboxItem)));
        }

        #region Methods

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if(e.ChangedButton == MouseButton.Left)
            {
                //Handle that the left mouse button was pressed
                object item = this;
                if(DataContext != null)
                {
                    item = DataContext;
                }

                RaiseEvent(new ToolboxItemEventArgs(ToolboxItemDragStartedEvent, this, item));
            }
        }

        #endregion Methods
    }
}
