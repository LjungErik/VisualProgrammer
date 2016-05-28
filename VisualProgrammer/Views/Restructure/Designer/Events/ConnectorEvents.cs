using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VisualProgrammer.Views.Restructure.Designer.Events
{
    public class ConnectorDragEventArgs : RoutedEventArgs
    {
        protected ConnectorDragEventArgs(RoutedEvent routedEvent, object sender)
            :base(routedEvent, sender)
        {
        }
    }

    public delegate void ConnectorDragEventHandler(object sender, ConnectorDragEventArgs e);

    public class ConnectorDragStartedEventArgs : ConnectorDragEventArgs
    {
        private bool cancel = false;

        public ConnectorDragStartedEventArgs(RoutedEvent routedEvent, object sender)
            :base(routedEvent, sender)
        {
        }

        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }
    }

    public delegate void ConnectorDragStartedEventHandler(object sender, ConnectorDragStartedEventArgs e);

    public class ConnectorDraggingEventArgs : ConnectorDragEventArgs
    {
        private double horizontalChange = 0.0;
        private double verticalChange = 0.0;

        public ConnectorDraggingEventArgs(RoutedEvent routedEvent, object sender, double horizontalChange, double verticalChange)
            :base(routedEvent, sender)
        {
            this.horizontalChange = horizontalChange;
            this.verticalChange = verticalChange;
        }

        public double HorizontalChange
        {
            get { return horizontalChange; }
        }

        public double VerticalChange
        {
            get { return verticalChange; }
        }
    }

    public delegate void ConnectorDraggingEventHander(object sender, ConnectorDraggingEventArgs e);

    public class ConnectorDragCompletedEventArgs : ConnectorDragEventArgs
    {
        public ConnectorDragCompletedEventArgs(RoutedEvent routedEvent, object sender)
            :base(routedEvent, sender)
        {
        }
    }

    public delegate void ConnectorDragCompletedEventHandler(object sender, ConnectorDragCompletedEventArgs e);
}
