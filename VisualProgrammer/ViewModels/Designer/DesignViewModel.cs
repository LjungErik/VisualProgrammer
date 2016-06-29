using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using VisualProgrammer.Utilities;
using VisualProgrammer.Data;
using VisualProgrammer.Data.Actions;

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
        private ObservableCollection<NodeViewModel> nodes = null;

        /// <summary>
        /// The collection of connections in the Designer.
        /// </summary>
        private ObservableCollection<ConnectionViewModel> connections = null;

        /// <summary>
        /// The start node in the Designer
        /// </summary>
        private StartNodeViewModel startNode = null;



        #endregion Internal Data Members

        public DesignViewModel(IList<Node> nodes, IList<Connection> connections)
        {
            this.nodes = GetObservableNodes(nodes);
            this.connections = GetObservableConnections(connections, this.nodes);
        }

        /// <summary>
        /// The collection of nodes in the Designer.
        /// </summary>
        public ObservableCollection<NodeViewModel> Nodes
        {
            get
            {
                if (nodes == null)
                {
                    nodes = new ObservableCollection<NodeViewModel>();
                }

                return nodes;
            }
        }

        /// <summary>
        /// The collection of connections in the Designer.
        /// </summary>
        public ObservableCollection<ConnectionViewModel> Connections
        {
            get
            {
                if (connections == null)
                {
                    connections = new ObservableCollection<ConnectionViewModel>();
                }

                return connections;
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

        private ObservableCollection<NodeViewModel> GetObservableNodes(IList<Node> nodes)
        {
            var observNodes = new ObservableCollection<NodeViewModel>();

            foreach(var node in nodes)
                observNodes.Add(GetNodeViewModel(node));

            return observNodes;
        }

        private ObservableCollection<ConnectionViewModel> GetObservableConnections(IList<Connection> connections, ObservableCollection<NodeViewModel> nodes)
        {
            var observConnection = new ObservableCollection<ConnectionViewModel>();

            foreach(var connection in connections)
                observConnection.Add(GetConnectionViewModel(connection, nodes));

            return observConnection;
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

        private ConnectionViewModel GetConnectionViewModel(Connection connection, ObservableCollection<NodeViewModel> nodes)
        {
            var sourceNode = nodes.Where(x => x.Model.NodeGuid == connection.SourceNodeGuid)
                                  .FirstOrDefault();
            var destNode = nodes.Where(x => x.Model.NodeGuid == connection.DestNodeGuid)
                                .FirstOrDefault();

            return sourceNode == null || destNode == null ?
                null :
                new ConnectionViewModel(connection, sourceNode, destNode);
        }

        #endregion Private Methods
    }
}
