using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows;
using VisualProgrammer.Utilities;

namespace VisualProgrammer.ViewModels.Designer
{
    public sealed class ConnectionViewModel : AbstractModelBase
    {
        #region Internal Data Members

        /// <summary>
        /// The source connector the connection is attached to.
        /// </summary>
        private ConnectorViewModel sourceConnector = null;

        /// <summary>
        /// The destination connector the connection is attached to.
        /// </summary>
        private ConnectorViewModel destConnector = null;

        /// <summary>
        /// The source and dest hotspots used for generating connection points.
        /// </summary>
        private Point sourceConnectorHotspot;
        private Point destConnectorHotspot;

        /// <summary>
        /// Points that make up the connection.
        /// </summary>
        private PointCollection points = null;

        /// <summary>
        /// If the current connection is visible in view
        /// </summary>
        private bool isVisible = true;

        #endregion Internal Data Members

        /// <summary>
        /// The source connector the connection is attached to.
        /// </summary>
        public ConnectorViewModel SourceConnector
        {
            get
            {
                return sourceConnector;
            }
            set
            {
                if (sourceConnector == value)
                {
                    return;
                }

                if (sourceConnector != null)
                {
                    sourceConnector.AttachedConnection = null;
                    sourceConnector.HotspotUpdated -= new EventHandler<EventArgs>(sourceConnector_HotspotUpdated);
                    sourceConnector.ParentNode.VisibilityChanged -= new EventHandler<VisibilityEventArgs>(sourceConnector_VisibilityChanged);
                }

                sourceConnector = value;

                if (sourceConnector != null)
                {
                    sourceConnector.AttachedConnection = this;
                    sourceConnector.HotspotUpdated += new EventHandler<EventArgs>(sourceConnector_HotspotUpdated);
                    sourceConnector.ParentNode.VisibilityChanged += new EventHandler<VisibilityEventArgs>(sourceConnector_VisibilityChanged);
                    this.SourceConnectorHotspot = sourceConnector.Hotspot;
                }

                OnPropertyChanged("SourceConnector");
                OnConnectionChanged();
            }
        }

        /// <summary>
        /// The destination connector the connection is attached to.
        /// </summary>
        public ConnectorViewModel DestConnector
        {
            get
            {
                return destConnector;
            }
            set
            {
                if (destConnector == value)
                {
                    return;
                }

                if (destConnector != null)
                {
                    destConnector.AttachedConnection = null;
                    destConnector.HotspotUpdated -= new EventHandler<EventArgs>(destConnector_HotspotUpdated);
                    destConnector.ParentNode.VisibilityChanged -= new EventHandler<VisibilityEventArgs>(destConnector_VisibilityChanged);
                }

                destConnector = value;

                if (destConnector != null)
                {
                    destConnector.AttachedConnection = this;
                    destConnector.HotspotUpdated += new EventHandler<EventArgs>(destConnector_HotspotUpdated);
                    destConnector.ParentNode.VisibilityChanged += new EventHandler<VisibilityEventArgs>(destConnector_VisibilityChanged);
                    this.DestConnectorHotspot = destConnector.Hotspot;
                }

                OnPropertyChanged("DestConnector");
                OnConnectionChanged();
            }
        }

        /// <summary>
        /// The source and dest hotspots used for generating connection points.
        /// </summary>
        public Point SourceConnectorHotspot
        {
            get
            {
                return sourceConnectorHotspot;
            }
            set
            {
                sourceConnectorHotspot = value;

                ComputeConnectionPoints();

                OnPropertyChanged("SourceConnectorHotspot");
            }
        }

        public Point DestConnectorHotspot
        {
            get
            {
                return destConnectorHotspot;
            }
            set
            {
                destConnectorHotspot = value;

                ComputeConnectionPoints();

                OnPropertyChanged("DestConnectorHotspot");
            }
        }

        /// <summary>
        /// Points that make up the connection.
        /// </summary>
        public PointCollection Points
        {
            get
            {
                return points;
            }
            set
            {
                points = value;

                OnPropertyChanged("Points");
            }
        }

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                if (isVisible == value)
                    return;

                isVisible = value;

                OnPropertyChanged("IsVisible");

                OnConnectionChanged();
            }
        }

        /// <summary>
        /// Event fired when the connection has changed.
        /// </summary>
        public event EventHandler<EventArgs> ConnectionChanged;

        #region Private Methods

        /// <summary>
        /// Raises the 'ConnectionChanged' event.
        /// </summary>
        private void OnConnectionChanged()
        {
            if (ConnectionChanged != null)
                ConnectionChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Event raised when the hotspot of the source connector has been updated.
        /// </summary>
        private void sourceConnector_HotspotUpdated(object sender, EventArgs e)
        {
            this.SourceConnectorHotspot = this.SourceConnector.Hotspot;
        }

        private void sourceConnector_VisibilityChanged(object sender, VisibilityEventArgs e)
        {
            IsVisible = e.Visibility;
        }

        /// <summary>
        /// Event raised when the hotspot of the dest connector has been updated.
        /// </summary>
        private void destConnector_HotspotUpdated(object sender, EventArgs e)
        {
            this.DestConnectorHotspot = this.DestConnector.Hotspot;
        }

        private void destConnector_VisibilityChanged(object sender, VisibilityEventArgs e)
        {
            IsVisible = e.Visibility;
        }

        /// <summary>
        /// Rebuild connection points.
        /// </summary>
        private void ComputeConnectionPoints()
        {
            PointCollection computedPoints = new PointCollection();
            computedPoints.Add(this.SourceConnectorHotspot);

            computedPoints.Add(this.DestConnectorHotspot);
            computedPoints.Freeze();

            this.Points = computedPoints;
        }

        #endregion Private Methods
    }
}
