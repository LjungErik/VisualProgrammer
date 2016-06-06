using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using VisualProgrammer.Utilities;
using VisualProgrammer.Views.Designer.Events;

namespace VisualProgrammer.Views.Designer
{
    public partial class DesignView
    {
        #region Private Data Members

        private Connector draggedOutConnector = null;

        private object draggedOutConnectorDataContext = null;

        private object draggedOutNodeDataContext = null;

        private object draggingConnectionDataContext = null;

        #endregion Private Data Members

        private void Connector_DragStarted(object sender, ConnectorDragStartedEventArgs e)
        {
            e.Handled = true;

            this.draggedOutConnector = (Connector)e.OriginalSource;
            var node = this.draggedOutConnector.ParentNode;
            this.draggedOutNodeDataContext = node.DataContext != null ? node.DataContext : node;
            this.draggedOutConnectorDataContext = this.draggedOutConnector.DataContext != null ? this.draggedOutConnector.DataContext : this.draggedOutConnector;

            ConnectionDragStartedEventArgs eventArgs = new ConnectionDragStartedEventArgs(ConnectionDragStartedEvent, this, this.draggedOutConnectorDataContext);
            RaiseEvent(eventArgs);

            this.draggingConnectionDataContext = eventArgs.Connection;

            if (draggingConnectionDataContext == null)
                e.Cancel = true;
        }

        private void Connector_Dragging(object sender, ConnectorDraggingEventArgs e)
        {
            e.Handled = true;

            Trace.Assert((Connector)e.OriginalSource == this.draggedOutConnector);

            var connectionDraggingEventArgs =
                new ConnectionDraggingEventArgs(ConnectionDraggingEvent, this, this.draggedOutConnectorDataContext, this.draggingConnectionDataContext);

            RaiseEvent(connectionDraggingEventArgs);
        }

        private void Connector_DragCompleted(object sender, ConnectorDragCompletedEventArgs e)
        {
            e.Handled = true;

            Trace.Assert((Connector)e.OriginalSource == this.draggedOutConnector);

            Point mousePoint = Mouse.GetPosition(this);

            Node nodeDraggedOver = null;
            object nodeDataContextDraggedOver = null;
            DetermineConnectorItemDraggedOver(mousePoint, out nodeDraggedOver, out nodeDataContextDraggedOver);

            RaiseEvent(new ConnectionDragCompletedEventArgs(ConnectionDragCompletedEvent, this, this.draggedOutConnectorDataContext, this.draggingConnectionDataContext, nodeDataContextDraggedOver));
        }

        private bool DetermineConnectorItemDraggedOver(Point hitPoint, out Node nodeItemDraggedOver, out object nodeDataContextDraggedOver)
        {
            nodeItemDraggedOver = null;
            nodeDataContextDraggedOver = null;

            //
            // Run a hit test 
            //
            HitTestResult result = null;
            VisualTreeHelper.HitTest(nodeControl, null,
                //
                // Result callback delegate.
                // This method is called when we have a result.
                //
                delegate(HitTestResult hitTestResult)
                {
                    result = hitTestResult;

                    return HitTestResultBehavior.Stop;
                },
                new PointHitTestParameters(hitPoint));

            if (result == null || result.VisualHit == null)
            {
                // Hit test failed.
                return false;
            }

            var hitItem = result.VisualHit as FrameworkElement;
            if (hitItem == null)
            {
                return false;
            }
            var nodeItem = WpfUtils.FindVisualParentWithType<Node>(hitItem);
            if (nodeItem == null)
            {
                return false;
            }

            var designerView = nodeItem.ParentDesignView;
            if (designerView != this)
            {
                return false;
            }

            object nodeDataContext = nodeItem;
            if (nodeItem.DataContext != null)
            {
                nodeDataContext = nodeItem.DataContext;
            }

            nodeItemDraggedOver = nodeItem;
            nodeDataContextDraggedOver = nodeDataContext;

            return true;
        }
    }
}
