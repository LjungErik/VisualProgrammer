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

    //public delegate void ToolboxItemDragStartedEventHandler(object sender, ToolboxItemEventArgs e);

    public delegate void ToolboxItemDropCanceledEventHandler(object sender, ToolboxItemEventArgs e);
}
