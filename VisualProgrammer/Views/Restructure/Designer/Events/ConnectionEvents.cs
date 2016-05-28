using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VisualProgrammer.Views.Restructure.Designer.Events
{
    public class ConnectionDragEventArgs : RoutedEventArgs
    {
        private object draggedOutConnector = null;
        protected object connection = null;

        protected ConnectionDragEventArgs(RoutedEvent routedEvent, object sender, object draggedOutConnector)
            :base(routedEvent, sender)
        {
            this.draggedOutConnector = draggedOutConnector;
        }

        public object ConnectorDraggedOut
        {
            get
            {
                return draggedOutConnector;
            }
        }
    }

    public class ConnectionDragStartedEventArgs : ConnectionDragEventArgs
    {
        public ConnectionDragStartedEventArgs(RoutedEvent routedEvent, object sender, object draggedOutConnector)
            :base(routedEvent, sender, draggedOutConnector)
        {
        }

        public object Connection
        {
            get
            {
                return connection;
            }
            set {
                connection = value;
            }
        }

    }

    public delegate void ConnectionDragStartedEventHandler(object sender, ConnectionDragStartedEventArgs e);

    public class ConnectionDraggingEventArgs : ConnectionDragEventArgs
    {
        public ConnectionDraggingEventArgs(RoutedEvent routedEvent, object sender, object draggedOutConnector, object connection)
            :base(routedEvent, sender, draggedOutConnector)
        {
            this.connection = connection;
        }

        public object Connection
        {
            get
            {
                return connection;
            }
        }
    }

    public delegate void ConnectionDraggingEventHandler(object sender, ConnectionDraggingEventArgs e);

    public class ConnectionDragCompletedEventArgs : ConnectionDragEventArgs
    {
        private object nodeDraggedOver = null;

        public ConnectionDragCompletedEventArgs(RoutedEvent routedEvent, object sender, object draggedOutConnector, object connection, object nodeDraggedOver)
            :base(routedEvent, sender, draggedOutConnector)
        {
            this.connection = connection;
            this.nodeDraggedOver = nodeDraggedOver;
        }

        public object NodeDraggedOver
        {
            get
            {
                return nodeDraggedOver;
            }
        }

        public object Connection
        {
            get
            {
                return connection;
            }
        }
    }

    public delegate void ConnectionDragCompletedEventHandler(object sender, ConnectionDragCompletedEventArgs e);
}
