﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using VisualProgrammer.Utilities;

namespace VisualProgrammer.ViewModels.Designer
{
    /* 
    * Copyright (c) 2012 Ashley Davis
    * --------------------------------------------------
    * Derived and Adapted from Ashley Davis article
    * "NetworkView: A WPF custom control for 
    * visualizing and editing networks, graphs 
    * and flow-charts".
    * --------------------------------------------------
    * This code was created by Ashley Davis, 2 Aug 2012
    * Licenced under the CPOL-License which is available
    * at the root of this project.
    * --------------------------------------------------
    * Modified on April 4 2016, by Erik Ljung
    */

    /// <summary>
    /// Defines a Designer of nodes and connections between the nodes.
    /// </summary>
    public sealed class DesignerViewModel
    {
        #region Internal Data Members

        /// <summary>
        /// The collection of nodes in the Designer.
        /// </summary>
        private ImpObservableCollection<NodeViewModel> nodes = null;

        /// <summary>
        /// The collection of connections in the Designer.
        /// </summary>
        private ImpObservableCollection<ConnectionViewModel> connections = null;

        /// <summary>
        /// The collection of temperary nodes in the Designer
        /// </summary>
        private ImpObservableCollection<NodeViewModel> temperaryNodes = null;

        /// <summary>
        /// The start node in the Designer
        /// </summary>
        private StartNodeViewModel startNode = null;

        /// <summary>
        /// The newly created active node
        /// </summary>
        private NodeViewModel newActiveNode = null;

        #endregion Internal Data Members

        /// <summary>
        /// The collection of nodes in the Designer.
        /// </summary>
        public ImpObservableCollection<NodeViewModel> Nodes
        {
            get
            {
                if (nodes == null)
                {
                    nodes = new ImpObservableCollection<NodeViewModel>();
                }

                return nodes;
            }
        }

        /// <summary>
        /// The collection of connections in the Designer.
        /// </summary>
        public ImpObservableCollection<ConnectionViewModel> Connections
        {
            get
            {
                if (connections == null)
                {
                    connections = new ImpObservableCollection<ConnectionViewModel>();
                    connections.ItemsRemoved += new EventHandler<CollectionItemsChangedEventArgs>(connections_ItemsRemoved);
                }

                return connections;
            }
        }

        /// <summary>
        /// The collection of temperary nodes in the Designer
        /// </summary>
        public ImpObservableCollection<NodeViewModel> TemperaryNodes
        {
            get
            {
                if(temperaryNodes == null)
                {
                    temperaryNodes = new ImpObservableCollection<NodeViewModel>();
                }

                return temperaryNodes;
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

        public NodeViewModel NewActiveNode
        {
            get
            {
                return newActiveNode;
            }
            set
            {
                if (newActiveNode == value)
                    return;
                
                if(newActiveNode != null)
                {
                    TemperaryNodes.Remove(newActiveNode);
                }

                newActiveNode = value;

                if(newActiveNode != null)
                {
                    TemperaryNodes.Add(newActiveNode);
                }

                
            }
        }

        #region Private Methods

        /// <summary>
        /// Event raised then Connections have been removed.
        /// </summary>
        private void connections_ItemsRemoved(object sender, CollectionItemsChangedEventArgs e)
        {
            foreach (ConnectionViewModel connection in e.Items)
            {
                connection.SourceConnector = null;
                connection.DestConnector = null;
            }
        }

        #endregion Private Methods
    }
}
