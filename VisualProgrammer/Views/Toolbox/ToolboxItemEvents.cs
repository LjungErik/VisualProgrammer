using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VisualProgrammer.Views.Toolbox
{
    public class ToolboxItemEventArgs : RoutedEventArgs
    {
        private object item = null;

        public ToolboxItemEventArgs(RoutedEvent routedEvent, object source)
            :base(routedEvent, source)
        {

        }

        public ToolboxItemEventArgs(RoutedEvent routedEvent, object source, object item)
            :base(routedEvent, source)
        {
            this.item = item;
        }

        public object Item
        {
            get { return item; }
        }
    }

    /// <summary>
    /// Event for when the Item is first dragged
    /// </summary>
    public delegate void ToolboxItemDragStartedEventHandler(object sender, ToolboxItemEventArgs e);

    /// <summary>
    /// Event for when the Item is dropped
    /// </summary>
    public delegate void ToolboxItemDroppedEventHandler(object sender, ToolboxItemEventArgs e);
}
