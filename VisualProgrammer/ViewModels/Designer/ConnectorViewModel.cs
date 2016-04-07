using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows;
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

    public sealed class ConnectorViewModel : AbstractModelBase
    {
        #region Internal Data Members

        /// <summary>
        /// The connection that is attached to this connector, or null if no connection is attached.
        /// </summary>
        private ConnectionViewModel attachedConnection = null;

        /// <summary>
        /// The hotspot (or center) of the connector.
        /// This is pushed through from ConnectorItem in the UI.
        /// </summary>
        private Point hotspot;

        #endregion Internal Data Members

        public ConnectorViewModel()
        {
            this.Type = ConnectorType.Undefined;
        }

        /// <summary>
        /// Defines the type of the connector.
        /// </summary>
        public ConnectorType Type
        {
            get;
            internal set;
        }

        /// <summary>
        /// Returns 'true' if the connector connected to another node.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (AttachedConnection != null && 
                    AttachedConnection.IsVisible &&
                    AttachedConnection.SourceConnector != null &&
                    AttachedConnection.DestConnector != null)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Returns 'true' if a connection is attached to the connector.
        /// The other end of the connection may or may not be attached to a node.
        /// </summary>
        public bool IsConnectionAttached
        {
            get
            {
                return AttachedConnection != null && AttachedConnection.IsVisible;
            }
        }

        /// <summary>
        /// The connection that is attached to this connector, or null if no connection is attached.
        /// </summary>
        public ConnectionViewModel AttachedConnection
        {
            get
            {
                return attachedConnection;
            }
            set
            {
                if (attachedConnection == value)
                    return;

                if(attachedConnection != null)
                {
                    attachedConnection.ConnectionChanged -= new EventHandler<EventArgs>(connection_ConnectionChanged);
                    OnConnectionDettached();
                    
                }

                attachedConnection = value;

                if(attachedConnection != null)
                {
                    attachedConnection.ConnectionChanged += new EventHandler<EventArgs>(connection_ConnectionChanged);
                    OnConnectionAttached(attachedConnection);
                }

                OnPropertyChanged("IsConnectionAttached");
                OnPropertyChanged("IsConnected");
            }
        }

        /// <summary>
        /// The parent node that the connector is attached to, or null if the connector is not attached to any node.
        /// </summary>
        public NodeViewModel ParentNode
        {
            get;
            internal set;
        }

        /// <summary>
        /// The hotspot (or center) of the connector.
        /// This is pushed through from ConnectorItem in the UI.
        /// </summary>
        public Point Hotspot
        {
            get
            {
                return hotspot;
            }
            set
            {
                if (hotspot == value)
                {
                    return;
                }

                hotspot = value;

                OnHotspotUpdated();
            }
        }

        /// <summary>
        /// Event raised when the connector hotspot has been updated.
        /// </summary>
        public event EventHandler<EventArgs> HotspotUpdated;

        #region Private Methods

        /// <summary>
        /// Event raised when a connection attached to the connector has changed.
        /// </summary>
        private void connection_ConnectionChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("IsConnectionAttached");
            OnPropertyChanged("IsConnected");
        }

        /// <summary>
        /// Called when the connector hotspot has been updated.
        /// </summary>
        private void OnHotspotUpdated()
        {
            OnPropertyChanged("Hotspot");

            if (HotspotUpdated != null)
            {
                HotspotUpdated(this, EventArgs.Empty);
            }
        }

        private void OnConnectionDettached()
        {
            if(ParentNode != null)
            {
                ParentNode.ConnectionDettached(this);
            }
        }

        private void OnConnectionAttached(ConnectionViewModel connection)
        {
            if (ParentNode != null && connection.SourceConnector != null && connection.SourceConnector.ParentNode != null)
            {
                ParentNode.ConnectionAttached(this, connection.SourceConnector.ParentNode);
            }
        }

        #endregion Private Methods
    }
}
