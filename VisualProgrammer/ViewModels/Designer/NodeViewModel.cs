using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using VisualProgrammer.Actions;
using VisualProgrammer.Utilities;

namespace VisualProgrammer.ViewModels.Designer
{
    public class VisibilityEventArgs : EventArgs
    {
        private bool visibility;

        public VisibilityEventArgs(bool visibility)
        {
            this.visibility = visibility; 
        }

        public bool Visibility
        {
            get
            {
                return this.visibility;
            }
        }
    }

    public abstract class NodeViewModel : AbstractModelBase
    {
        #region CONSTANT

        private const double DEFAULT_OPACITY = 0.5;

        #endregion CONSTANT

        #region Private Data Members

        /// <summary>
        /// The X coordinate for the position of the node.
        /// </summary>
        private double x = 0;

        /// <summary>
        /// The Y coordinate for the position of the node.
        /// </summary>
        private double y = 0;

        /// <summary>
        /// The Z index of the node.
        /// </summary>
        private int zIndex = 0;

        /// <summary>
        /// List of input connectors (connections points) attached to the node.
        /// </summary>
        private ConnectorViewModel inputConnector = null;

        /// <summary>
        /// List of output connectors (connections points) attached to the node.
        /// </summary>
        private ConnectorViewModel outputConnector = null;

        /// <summary>
        /// Set to 'true' when the node is selected.
        /// </summary>
        private bool isSelected = false;

        /// <summary>
        /// The visibility of the node (dragged outside)
        /// </summary>
        private bool isVisible = true;

        private Point startLocation = new Point(0, 0);

        #endregion Private Data Members

        public NodeViewModel()
        {
        }

        /// <summary>
        /// The X coordinate for the position of the node.
        /// </summary>
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                if (x == value)
                {
                    return;
                }

                x = value;

                OnPropertyChanged("X");
            }
        }

        /// <summary>
        /// The Y coordinate for the position of the node.
        /// </summary>
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                if (y == value)
                {
                    return;
                }

                y = value;

                OnPropertyChanged("Y");
            }
        }

        public virtual double Width
        {
            get
            {
                return 0.0;
            }
        }

        public virtual double Height
        {
            get
            {
                return 0.0;
            }
        }

        /// <summary>
        /// The Z index of the node.
        /// </summary>
        public int ZIndex
        {
            get
            {
                return zIndex;
            }
            set
            {
                if (zIndex == value)
                {
                    return;
                }

                zIndex = value;

                OnPropertyChanged("ZIndex");
            }
        }

        /// <summary>
        /// The Model that represents the action of the node
        /// </summary>
        public virtual IRobotAction Model
        {
            get { return null; }
        }

        /// <summary>
        /// The status if the node is visible or not
        /// </summary>
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

                if(VisibilityChanged != null)
                {
                    VisibilityChanged(this, new VisibilityEventArgs(isVisible));
                }
            }
        }

        public event EventHandler<VisibilityEventArgs> VisibilityChanged;

        /// <summary>
        /// Input connector attached to the node.
        /// </summary>
        public virtual ConnectorViewModel InputConnector
        {
            get
            {
                return inputConnector;
            }
            set
            {
                if (inputConnector == value)
                    return;

                if(inputConnector != null)
                {
                    inputConnector.ParentNode = null;
                }

                inputConnector = value;

                if(inputConnector != null)
                {
                    inputConnector.ParentNode = this;
                }
            }
        }

        /// <summary>
        /// Output connector attached to the node.
        /// </summary>
        public virtual ConnectorViewModel OutputConnector
        {
            get
            {
                return outputConnector;
            }
            set
            {
                if (outputConnector == value)
                    return;

                if(outputConnector != null)
                {
                    outputConnector.ParentNode = null;
                }

                outputConnector = value;

                if(outputConnector != null)
                {
                    outputConnector.ParentNode = this;
                }
            }
        }

        /// <summary>
        /// A helper property that retrieves a list of all connections attached to the node (input and output). 
        /// </summary>
        public virtual ICollection<ConnectionViewModel> AttachedConnections
        {
            get
            {
                List<ConnectionViewModel> attachedConnections = new List<ConnectionViewModel>();

                attachedConnections.Add(InputConnector.AttachedConnection);

                attachedConnections.Add(OutputConnector.AttachedConnection);

                return attachedConnections;
            }
        }

        /// <summary>
        /// Set to 'true' when the node is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (isSelected == value)
                {
                    return;
                }

                isSelected = value;

                OnPropertyChanged("IsSelected");
            }
        }

        public void SaveLocation()
        {
            startLocation.X = this.X;
            startLocation.Y = this.Y;
        }

        public void ResetLocation()
        {
            this.X = startLocation.X;
            this.Y = startLocation.Y;
        }
    }
}
