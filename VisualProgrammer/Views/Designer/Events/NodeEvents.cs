using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VisualProgrammer.Views.Designer.Events
{
    public class NodeDragEventArgs : RoutedEventArgs
    {
        private ICollection nodes = null;

        protected NodeDragEventArgs(RoutedEvent routedEvent, object sender, ICollection nodes)
            : base(routedEvent, sender)
        {
            this.nodes = nodes;
        }

        public ICollection Nodes
        {
            get
            {
                return nodes;
            }
        }
    }

    public class NodeDragStartedEventArgs : NodeDragEventArgs
    {
        private bool cancel = false;

        public NodeDragStartedEventArgs(RoutedEvent routedEvent, object sender, ICollection nodes)
            : base(routedEvent, sender, nodes)
        { }

        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }
    }

    public delegate void NodeDragStartedEventHandler(object sender, NodeDragStartedEventArgs e);

    public class NodeDraggingEventArgs : NodeDragEventArgs
    {
        private double horizontalChange = 0;
        private double verticalChange = 0;

        public NodeDraggingEventArgs(RoutedEvent routedEvent, object sender, ICollection nodes, double horizontalChange, double verticalChange)
            : base(routedEvent, sender, nodes)
        {
            this.horizontalChange = horizontalChange;
            this.verticalChange = verticalChange;
        }

        public double HorizontalChange
        {
            get
            {
                return horizontalChange;
            }
        }

        public double VerticalChange
        {
            get
            {
                return verticalChange;
            }
        }
    }

    public delegate void NodeDraggingEventHandler(object sender, NodeDraggingEventArgs e);

    public class NodeDragCompletedEventArgs : NodeDragEventArgs
    {
        private bool cancel = false;

        public NodeDragCompletedEventArgs(RoutedEvent routedEvent, object sender, ICollection nodes)
            : base(routedEvent, sender, nodes)
        {
        }

        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }
    }

    public delegate void NodeDragCompletedEventHandler(object sender, NodeDragCompletedEventArgs e);
}
