using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisualProgrammer.ViewModels;
using VisualProgrammer.ViewModels.Designer;
using VisualProgrammer.ViewModels.Toolbox;
using VisualProgrammer.Views.Designer;
using VisualProgrammer.Views.Designer.Events;
using VisualProgrammer.Views.Toolbox;

namespace VisualProgrammer
{
    /// <summary>
    /// Interaction logic for DesignerControl.xaml
    /// </summary>
    public partial class DesignerControl : UserControl
    {

        public DesignerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Convenient accessor for the view-model.
        /// </summary>
        public DesignerControlViewModel ViewModel
        {
            get
            {
                return (DesignerControlViewModel)DataContext;
            }
        }

        /// <summary>
        /// Event raised when the user has started to drag out a connection.
        /// </summary>
        private void designerControl_ConnectionDragStarted(object sender, ConnectionDragStartedEventArgs e)
        {
            var draggedOutConnector = (ConnectorViewModel)e.ConnectorDraggedOut;
            var curDragPoint = Mouse.GetPosition(designerControl);

            var connection = this.ViewModel.ConnectionDragStarted(draggedOutConnector, curDragPoint);

            e.Connection = connection;
        }

        /// <summary>
        /// Event raised while the user is dragging a connection.
        /// </summary>
        private void designerControl_ConnectionDragging(object sender, ConnectionDraggingEventArgs e)
        {
            Point curDragPoint = Mouse.GetPosition(designerControl);
            var connection = (ConnectionViewModel)e.Connection;
            this.ViewModel.ConnectionDragging(curDragPoint, connection);
        }

        /// <summary>
        /// Event raised when the user has finished dragging out a connection.
        /// </summary>
        private void designerControl_ConnectionDragCompleted(object sender, ConnectionDragCompletedEventArgs e)
        {
            var connectorDraggedOut = (ConnectorViewModel)e.ConnectorDraggedOut;
            var nodeDraggedOver = (NodeViewModel)e.NodeDraggedOver;
            var newConnection = (ConnectionViewModel)e.Connection;
            this.ViewModel.ConnectionDragCompleted(newConnection, connectorDraggedOut, nodeDraggedOver);
        }

        /// <summary>
        /// Event rasied when the user has started dragging a node
        /// </summary>
        private void designerControl_NodeDragStarted(object sender, NodeDragStartedEventArgs e)
        {
            toolboxView.IsNodeDragged = true;
        }

        /// <summary>
        /// Event rasied when the user is dragging a node
        /// </summary>
        private void designerControl_NodeDragging(object sender, NodeDraggingEventArgs e)
        {
            Point mouse = Mouse.GetPosition(toolboxView);
            
            if (HitTest(mouse, toolboxView))    //Check if dropped on toolbox
            {
                if (!toolboxView.IsNodeDraggedOver)
                {
                    foreach (NodeViewModel node in e.Nodes)
                    {
                        node.IsVisible = false;
                    }
                }

                toolboxView.IsNodeDraggedOver = true;

            }
            else if (toolboxView.IsNodeDraggedOver)
            {
                toolboxView.IsNodeDraggedOver = false;

                foreach (NodeViewModel node in e.Nodes)
                {
                    node.IsVisible = true;
                }
            }

            SetCursor();
        }

        /// <summary>
        /// Event rasied when the user has finished dragging a node
        /// </summary>
        private void designerControl_NodeDragCompleted(object sender, NodeDragCompletedEventArgs e)
        {
            Point mouse = Mouse.GetPosition(toolboxView);

            if (HitTest(mouse, toolboxView))
            {
                NodeViewModel[] nodes = new NodeViewModel[e.Nodes.Count];
                e.Nodes.CopyTo(nodes, 0);
                nodes.ToList().ForEach(x => ViewModel.RemoveNode(x));
            }

            e.Cancel = IsOutOfBounds();

            toolboxView.IsNodeDraggedOver = false;
            toolboxView.IsNodeDragged = false;
            Mouse.OverrideCursor = null;
        }

        private bool HitTest(Point hitPoint, FrameworkElement element)
        {
            if ((element.ActualHeight > hitPoint.Y && hitPoint.Y >= 0) && (element.ActualWidth > hitPoint.X && hitPoint.X >= 0))
                return true;
            return false;
        }

        private void SetCursor()
        {
            if(IsOutOfBounds())
                Mouse.OverrideCursor = Cursors.No;
            else
                Mouse.OverrideCursor = null;
        }

        private bool IsOutOfBounds()
        {
            Point mousePosition = Mouse.GetPosition(this);
            return !HitTest(mousePosition, this);
        }

        private void toolboxView_DraggedOver(object sender, DragDropEventArgs e)
        {
            Mouse.OverrideCursor = null;
        }

        private void designerControl_DraggedOver(object sender, DragDropEventArgs e)
        {
            Mouse.OverrideCursor = null;
        }

        private void designerControl_DroppedOver(object sender, DragDropEventArgs e)
        {
            NodeViewModel retItem = null;

            var toolItem = e.DraggedItem as ToolboxItem;
            if(toolItem != null)
            {
                var toolDataContext = (ToolboxItemViewModel)toolItem.DataContext;
                var mouseLocation = Mouse.GetPosition(designerControl);
                retItem = ViewModel.DropNode(toolDataContext, mouseLocation);
            }
            e.ReturnItem = retItem;
        }
    }
}
