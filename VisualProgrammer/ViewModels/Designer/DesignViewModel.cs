using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    }
}
