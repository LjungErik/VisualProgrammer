
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using VisualProgrammer.Data;
using VisualProgrammer.Data.Actions;
using VisualProgrammer.Utilities;

namespace VisualProgrammer.ViewModels.Designer
{
    /// <summary>
    /// Defines a Designer of nodes and connections between the nodes.
    /// </summary>
    public sealed class DesignViewModel
    {
        #region Internal Data Members

        /// <summary>
        /// The collection of nodes in the Designer.
        /// </summary>
        private ChangeableCollection<NodeViewModel> nodes = null;

        /// <summary>
        /// The collection of connections in the Designer.
        /// </summary>
        private ChangeableCollection<ConnectionViewModel> connections = null;

        private ChangeableCollection<ConnectionViewModel> tempconnection = null;

        /// <summary>
        /// The start node in the Designer
        /// </summary>
        private StartNodeViewModel startNode = null;

        #endregion Internal Data Members

        public event EventHandler<EventArgs> PropertyChanged;

        public DesignViewModel(IList<Node> nodeItems, IList<Connection> connectionItems)
        {
            SetNodes(nodeItems);
            SetConnections(connectionItems);

            tempconnection = new ChangeableCollection<ConnectionViewModel>();

            nodes.ContentChanged += new PropertyChangedEventHandler(OnContentChanged);
            connections.ContentChanged += new PropertyChangedEventHandler(OnContentChanged);
        }

        /// <summary>
        /// The collection of nodes in the Designer.
        /// </summary>
        public ChangeableCollection<NodeViewModel> Nodes
        {
            get
            {
                return nodes;
            }
        }

        /// <summary>
        /// The collection of connections in the Designer.
        /// </summary>
        public ChangeableCollection<ConnectionViewModel> Connections
        {
            get
            {
                return connections;
            }
        }

        /// <summary>
        /// Holds a connection that is currently being created/dragged on the screen
        /// </summary>
        public ObservableCollection<ConnectionViewModel> TemperaryConnection
        {
            get
            {
                return tempconnection;
            }
        }

        public StartNodeViewModel StartNode
        {
            get
            {
                return startNode;
            }
            set
            {
                if (startNode == value)
                    return;

                startNode = value;
            }
        }

        #region Private Methods

        private void SetNodes(IList<Node> nodeItems)
        {
            nodes = new ChangeableCollection<NodeViewModel>();

            foreach (var nodeItem in nodeItems)
            {
                var node = GetNodeViewModel(nodeItem);
                nodes.Add(node);

                if (StartNode == null && node.GetType() == typeof(StartNodeViewModel))
                    StartNode = (StartNodeViewModel)node;
            }

        }

        private void SetConnections(IList<Connection> connectionItems)
        {
            connections = new ChangeableCollection<ConnectionViewModel>();

            foreach(var connectionItem in connectionItems)
            {
                var connection = GetConnectionViewModel(connectionItem);

                if(connection != null)
                    connections.Add(connection);

            }
        }

        private NodeViewModel GetNodeViewModel(Node node)
        {
            if (node.Action is ServoMoveAction)
                return new ServoMoveNodeViewModel(node);
            else if (node.Action is SleepAction)
                return new SleepNodeViewModel(node);
            else if (node.Action is StartAction)
                return new StartNodeViewModel(node);
            else if (node.Action is UARTSendAction)
                return new UARTSendNodeViewModel(node);

            return null;
        }

        private ConnectionViewModel GetConnectionViewModel(Connection connection)
        {
            if (nodes == null)
                return null;

            var sourceNode = nodes.Where(x => x.Model.NodeGuid == connection.SourceNodeGuid)
                                  .FirstOrDefault();
            var destNode = nodes.Where(x => x.Model.NodeGuid == connection.DestNodeGuid)
                                .FirstOrDefault();

            return sourceNode == null || destNode == null ?
                null :
                new ConnectionViewModel(connection, sourceNode, destNode);
        }

        private void OnContentChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, EventArgs.Empty);
        }

        #endregion Private Methods
    }
}
