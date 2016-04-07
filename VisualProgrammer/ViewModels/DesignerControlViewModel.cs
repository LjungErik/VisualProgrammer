using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Windows.Input;
using VisualProgrammer.Utilities;
using VisualProgrammer.ViewModels.Designer;
using VisualProgrammer.ViewModels.Toolbox;
using VisualProgrammer.Actions;
using VisualProgrammer.Processing;
using VisualProgrammer.Processing.File;
using VisualProgrammer.Processing.Commands.Shell;
using VisualProgrammer.Processing.Commands.Compiler;

namespace VisualProgrammer.ViewModels
{
    /// <summary>
    /// The view-model for the main window.
    /// </summary>
    public class DesignerControlViewModel : AbstractModelBase
    {
        #region Internal Data Members

        /// <summary>
        /// This is the designer that is displayed in the window.
        /// It is the main part of the view-model.
        /// </summary>
        public DesignerViewModel designer = null;

        public ToolboxViewModel toolbox = null;

        ///
        /// The current scale at which the content is being viewed.
        /// 
        private double contentScale = 1;

        ///
        /// The X coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        private double contentOffsetX = 0;

        ///
        /// The Y coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        private double contentOffsetY = 0;

        ///
        /// The width of the content (in content coordinates).
        /// 
        private double contentWidth = 1000;

        ///
        /// The heigth of the content (in content coordinates).
        /// 
        private double contentHeight = 1000;

        #endregion Internal Data Members

        public DesignerControlViewModel()
        {
            // Add some test data to the view-model.
            PopulateWithTestData();
        }

        /// <summary>
        /// This is the Designer that is displayed in the window.
        /// It is the main part of the view-model.
        /// </summary>
        public DesignerViewModel Designer
        {
            get
            {
                return designer;
            }
            set
            {
                designer = value;

                OnPropertyChanged("Designer");
            }
        }

        public ToolboxViewModel Toolbox
        {
            get
            {
                return toolbox;
            }
            set
            {
                toolbox = value;

                OnPropertyChanged("Toolbox");
            }
        }

        ///
        /// The current scale at which the content is being viewed.
        /// 
        public double ContentScale
        {
            get
            {
                return contentScale;
            }
            set
            {
                contentScale = value;

                OnPropertyChanged("ContentScale");
            }
        }

        ///
        /// The X coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        public double ContentOffsetX
        {
            get
            {
                return contentOffsetX;
            }
            set
            {
                contentOffsetX = value;

                OnPropertyChanged("ContentOffsetX");
            }
        }

        ///
        /// The Y coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        public double ContentOffsetY
        {
            get
            {
                return contentOffsetY;
            }
            set
            {
                contentOffsetY = value;

                OnPropertyChanged("ContentOffsetY");
            }
        }

        ///
        /// The width of the content (in content coordinates).
        /// 
        public double ContentWidth
        {
            get
            {
                return contentWidth;
            }
            set
            {
                contentWidth = value;

                OnPropertyChanged("ContentWidth");
            }
        }

        ///
        /// The heigth of the content (in content coordinates).
        /// 
        public double ContentHeight
        {
            get
            {
                return contentHeight;
            }
            set
            {
                contentHeight = value;

                OnPropertyChanged("ContentHeight");
            }
        }

        /// <summary>
        /// Called when the user has started to drag out a connector, thus creating a new connection.
        /// </summary>
        public ConnectionViewModel ConnectionDragStarted(ConnectorViewModel draggedOutConnector, Point curDragPoint)
        {
            var existingConnection = draggedOutConnector.AttachedConnection;
            if(existingConnection != null)
            {
                //
                // Remove any existing connection so that there
                // is only one connection going from the connector
                //
                this.Designer.Connections.Remove(existingConnection);
            }

            //
            // Create a new connection to add to the view-model.
            //
            var connection = new ConnectionViewModel();

            //
            // The user is dragging out a source connector (an output) and will connect it to a destination connector (an input).
            //
            connection.SourceConnector = draggedOutConnector;
            connection.DestConnectorHotspot = curDragPoint;
            //
            // Add the new connection to the view-model.
            //
            this.Designer.Connections.Add(connection);

            return connection;
        }

        /// <summary>
        /// Called as the user continues to drag the connection.
        /// </summary>
        public void ConnectionDragging(Point curDragPoint, ConnectionViewModel connection)
        {
            if (connection.DestConnector == null)
            {
                connection.DestConnectorHotspot = curDragPoint;
            }
            else
            {
                connection.SourceConnectorHotspot = curDragPoint;
            }
        }

        /// <summary>
        /// Called when the user has finished dragging out the new connection.
        /// </summary>
        public void ConnectionDragCompleted(ConnectionViewModel newConnection, ConnectorViewModel connectorDraggedOut, NodeViewModel nodeDraggedOver)
        {
            if (nodeDraggedOver == null || nodeDraggedOver.InputConnector == null)
            {
                this.Designer.Connections.Remove(newConnection);
                return;
            }

            var inputConnector = nodeDraggedOver.InputConnector;

            if(FindLoop(connectorDraggedOut, inputConnector))
            {
                this.Designer.Connections.Remove(newConnection);
                return;
            }

            var existingConnection = inputConnector.AttachedConnection;
            if (existingConnection != null)
            {
                this.Designer.Connections.Remove(existingConnection);
            }

            newConnection.DestConnector = inputConnector;
        }

        /// <summary>
        /// Create a node and add it to the view-model.
        /// </summary>
        public NodeViewModel CreateNode(Point nodeLocation, bool centerNode)
        {
            var node = new UARTSendNodeViewModel();
            node.X = nodeLocation.X;
            node.Y = nodeLocation.Y;

            node.InputConnector = new ConnectorViewModel();
            node.OutputConnector = new ConnectorViewModel();

            if (centerNode)
            {

                EventHandler<EventArgs> sizeChangedEventHandler = null;
                sizeChangedEventHandler =
                    delegate(object sender, EventArgs e)
                    {
                        node.X -= node.Size.Width / 2;
                        node.Y -= node.Size.Height / 2;

                        node.SizeChanged -= sizeChangedEventHandler;
                    };

                node.SizeChanged += sizeChangedEventHandler;
            }

            this.Designer.Nodes.Add(node);

            return node;
        }

        public NodeViewModel CreateStartNode(Point nodeLocation, bool centerNode)
        {
            var node = new StartNodeViewModel();
            node.X = nodeLocation.X;
            node.Y = nodeLocation.Y;

            node.OutputConnector = new ConnectorViewModel();

            this.Designer.Nodes.Add(node);
            this.Designer.StartNode = node;
            return node;
        }

        public void RemoveNode(NodeViewModel node)
        {
            //Remove the attached connections
            node.AttachedConnections.ToList().ForEach(x => this.Designer.Connections.Remove(x));

            //Remove 
            this.Designer.Nodes.Remove(node);
        }

        /// <summary>
        /// Build the virtual program
        /// </summary>
        public bool PerformBuild()
        {
            List<IRobotAction> actions = ModelCollector.GetModels(this.Designer.StartNode);

            Parser parser = new Parser(new FileWriter());
            parser.GenrateCode(actions, "Testname.c");

            Compiler compiler = new Compiler(actions,
                            new CommandShell(), 
                            new CompilerCommand());

            compiler.Execute("Testname");


            return false;
        }


        #region Private Methods

        private bool FindLoop(ConnectorViewModel outputConnector, ConnectorViewModel inputConnector)
        {
            Trace.Assert(outputConnector.Type != inputConnector.Type);

            NodeViewModel outputNode = outputConnector.ParentNode;
            NodeViewModel currentNode = inputConnector.ParentNode;
            while (currentNode != null)
            {
                if (outputNode == currentNode)
                    return true;

                if (currentNode.OutputConnector.IsConnected)
                {
                    ConnectionViewModel connection = currentNode.OutputConnector.AttachedConnection;
                    currentNode = connection.DestConnector.ParentNode;
                }
                else
                {
                    currentNode = null;
                }
            }

            return false;
        }

        /// <summary>
        /// A function to conveniently populate the view-model with test data.
        /// </summary>
        private void PopulateWithTestData()
        {
            this.Designer = new DesignerViewModel();
            this.Toolbox = new ToolboxViewModel();

            CreateStartNode(new Point(50, 150), false);
            CreateNode(new Point(150, 50), false);
        }

        #endregion Private Methods

        internal void ToolboxItemDragStarted(ToolboxItemViewModel tool)
        {
            //Get the node corresponding to the clicked tool
            NodeViewModel node = tool.GetNode();
            node.X = 0;
            node.Y = 0;

            //Set the currently Active node (generates its size 
            //for use later in DesignerViewMouseEnter)
            this.Designer.NewActiveNode = node;
        }

        internal void ToolboxItemDropped()
        {
            this.Designer.NewActiveNode = null;
        }

        internal void DesignerViewMouseEnter(Point mousePosition)
        {
            if (this.Designer.NewActiveNode != null)
            {
                this.Designer.NewActiveNode.X = mousePosition.X - this.Designer.NewActiveNode.Size.Width / 2;
                this.Designer.NewActiveNode.Y = mousePosition.Y - this.Designer.NewActiveNode.Size.Height / 2;

                //Trigger a external mouse press on node (Activate dragging)
                this.Designer.NewActiveNode.IsPressed = true;

                this.Designer.Nodes.Add(this.Designer.NewActiveNode);

                this.Designer.NewActiveNode = null;
            }
        }
    }
}
