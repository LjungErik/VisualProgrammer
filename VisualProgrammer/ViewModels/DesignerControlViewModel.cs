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
using VisualProgrammer.Utilities.Processing;
using VisualProgrammer.Utilities.Processing.FileHelpers;
using VisualProgrammer.Utilities.Processing.Commands.Shell;
using VisualProgrammer.Utilities.Processing.Commands.Compiler;
using VisualProgrammer.Data;
using System.Collections.ObjectModel;

namespace VisualProgrammer.ViewModels
{
    /// <summary>
    /// The view-model for the main window.
    /// </summary>
    public class DesignerControlViewModel : AbstractModelBase
    {
        #region Internal Data Members

        public DesignViewModel designer = null;

        public ToolboxViewModel toolbox = null;

        #endregion Internal Data Members

        public DesignerControlViewModel(VisualProject project)
        {
            this.Toolbox = new ToolboxViewModel();
            this.Designer = new DesignViewModel(project.Nodes, project.Connections);
        }

        public DesignViewModel Designer
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

        /// <summary>
        /// Called when the user has started to drag out a connector, thus creating a new connection.
        /// </summary>
        public ConnectionViewModel ConnectionDragStarted(ConnectorViewModel draggedOutConnector, Point curDragPoint)
        {
            var existingConnection = draggedOutConnector.AttachedConnection;
            if(existingConnection != null)
                this.Designer.Connections.Remove(existingConnection);

            var connection = new ConnectionViewModel();

            connection.SourceConnector = draggedOutConnector;
            connection.DestConnectorHotspot = curDragPoint;

            this.Designer.Connections.Add(connection);

            return connection;
        }

        /// <summary>
        /// Called as the user continues to drag the connection.
        /// </summary>
        public void ConnectionDragging(Point curDragPoint, ConnectionViewModel connection)
        {
            if (connection.DestConnector == null)
                connection.DestConnectorHotspot = curDragPoint;
            else
                connection.SourceConnectorHotspot = curDragPoint;
        }

        /// <summary>
        /// Called when the user has finished dragging out the new connection.
        /// </summary>
        public void ConnectionDragCompleted(ConnectionViewModel newConnection, ConnectorViewModel connectorDraggedOut, NodeViewModel nodeDraggedOver)
        {
            if (nodeDraggedOver == null || nodeDraggedOver.InputConnector == null)
            {
                connectorDraggedOut.AttachedConnection = null;
                this.Designer.Connections.Remove(newConnection);
                return;
            }

            var inputConnector = nodeDraggedOver.InputConnector;

            if(FindLoop(connectorDraggedOut, inputConnector))
            {
                connectorDraggedOut.AttachedConnection = null;
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

        public void RemoveNode(NodeViewModel node)
        {
            //Remove the attached connections
            node.AttachedConnections.ToList().ForEach(x => this.Designer.Connections.Remove(x));

            //Remove 
            this.Designer.Nodes.Remove(node);

            //Remove as startnode 
            if(node is StartNodeViewModel)
                this.Designer.StartNode = null;
        }

        #region Private Methods

        private bool FindLoop(ConnectorViewModel outputConnector, ConnectorViewModel inputConnector)
        {

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

        private VisualProject GetProject()
        {
            var project = new VisualProject();

            project.Nodes = Designer.Nodes.Select(x => x.Model).ToList();
            project.Connections = Designer.Connections.Select(x => x.Model).ToList();

            return project;
        }

        #endregion Private Methods

        public NodeViewModel DropNode(ToolboxItemViewModel tool, Point mousePosition)
        {
            NodeViewModel newNode = tool.GetNode();

            if (newNode is StartNodeViewModel && this.Designer.StartNode != null)
                return null;

            newNode.X = mousePosition.X - (newNode.Width / 2);
            newNode.Y = mousePosition.Y - (newNode.Height / 2);

            this.Designer.Nodes.Add(newNode);

            if (newNode is StartNodeViewModel)
                this.Designer.StartNode = (StartNodeViewModel)newNode;

            return newNode;
        }
    }
}
