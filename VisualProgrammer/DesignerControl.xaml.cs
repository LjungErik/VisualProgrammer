using System;
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

            //
            // Delegate the real work to the view model.
            //
            var connection = this.ViewModel.ConnectionDragStarted(draggedOutConnector, curDragPoint);

            //
            // Must return the view-model object that represents the connection via the event args.
            // This is so that DesignerView can keep track of the object while it is being dragged.
            //
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
            //TODO
            toolboxView.IsNodeDragged = true;
        }

        /// <summary>
        /// Event rasied when the user is dragging a node
        /// </summary>
        private void designerControl_NodeDragging(object sender, NodeDraggingEventArgs e)
        {
            //TODO
            Point mouse = Mouse.GetPosition(toolboxView);
            //Check if dropped on toolbox
            if (HitTest(mouse, toolboxView))
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
        }

        /// <summary>
        /// Event rasied when the user has finished dragging a node
        /// </summary>
        private void designerControl_NodeDragCompleted(object sender, NodeDragCompletedEventArgs e)
        {

            Point mouse = Mouse.GetPosition(toolboxView);
            //Check if dropped on toolbox
            if (HitTest(mouse, toolboxView))
            {
                NodeViewModel[] nodes = new NodeViewModel[e.Nodes.Count];
                e.Nodes.CopyTo(nodes, 0);
                nodes.ToList().ForEach(x => ViewModel.RemoveNode(x));
            }

            //Update the toolbox
            toolboxView.IsNodeDraggedOver = false;
            toolboxView.IsNodeDragged = false;
        }

        /// <summary>
        /// Event that is rasied when the user has entered the DesignView with the cursor
        /// </summary>
        private void designerControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Point mouseLocation = Mouse.GetPosition(designerControl);

            this.ViewModel.DesignerViewMouseEnter(mouseLocation);
        }

        private bool HitTest(Point hitPoint, FrameworkElement element)
        {
            if ((element.ActualHeight > hitPoint.Y && hitPoint.Y >= 0) && (element.ActualWidth > hitPoint.X && hitPoint.X >= 0))
                return true;
            return false;
        }

        /// <summary>
        /// Event raised to create a new node.
        /// </summary>
        private void CreateNode_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CreateNode();
        }

        /// <summary>
        /// Creates a new node in the designer at the current mouse location.
        /// </summary>
        private void CreateNode()
        {
            var newNodePosition = Mouse.GetPosition(designerControl);
            this.ViewModel.CreateNode(newNodePosition, true);
        }

        /// <summary>
        /// Event raised when the size of a node has changed.
        /// </summary>
        private void Node_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //
            // The size of a node, as determined in the UI by the node's data-template,
            // has changed.  Push the size of the node through to the view-model.
            //
            var element = (FrameworkElement)sender;
            var node = (NodeViewModel)element.DataContext;
            node.Size = new Size(element.ActualWidth, element.ActualHeight);
        }

        private void ToolboxView_ItemDragStarted(object sender, ToolboxItemEventArgs e)
        {
            var tool = (ToolboxItemViewModel)e.Item;
            this.ViewModel.ToolboxItemDragStarted(tool);
        }

        private void ToolboxView_ItemDropped(object sender, ToolboxItemEventArgs e)
        {
            this.ViewModel.ToolboxItemDropped();
        }

        private void buildBtnClicked(object sender, RoutedEventArgs e)
        {
            //TODO: Add code for building the real code
            //Open compiler status window with the provided startnode
            CompilerStatusWindow compileStatusWindow = new CompilerStatusWindow(this.ViewModel.Designer.StartNode);
            compileStatusWindow.Owner = Window.GetWindow(this);
            compileStatusWindow.ShowDialog();
        }
    }
}
