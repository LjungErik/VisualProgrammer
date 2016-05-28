using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Input;
using VisualProgrammer.Utilities;

namespace VisualProgrammer.Views.Designer
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
   */

    /// <summary>
    /// Partial definition of the DesignerView class.
    /// This file only contains private members related to dragging connections.
    /// </summary>
    public partial class DesignerView
    {
        #region Private Data Members

        /// <summary>
        /// When dragging a connection, this is set to the ConnectorItem that was initially dragged out.
        /// </summary>
        private ConnectorItem draggedOutConnectorItem = null;

        /// <summary>
        /// The view-model object for the connector that has been dragged out.
        /// </summary>
        private object draggedOutConnectorDataContext = null;

        /// <summary>
        /// The view-model object for the node whose connector was dragged out.
        /// </summary>
        private object draggedOutNodeDataContext = null;

        /// <summary>
        /// The view-model object for the connection that is currently being dragged, or null if none being dragged.
        /// </summary>
        private object draggingConnectionDataContext = null;

        #endregion Private Data Members

        #region Private Methods

        /// <summary>
        /// Event raised when the user starts to drag a connector.
        /// </summary>
        private void ConnectorItem_DragStarted(object source, ConnectorItemDragStartedEventArgs e)
        {
            this.Focus();

            e.Handled = true;

            this.IsDragging = true;
            this.IsNotDragging = false;
            this.IsDraggingConnection = true;
            this.IsNotDraggingConnection = false;

            this.draggedOutConnectorItem = (ConnectorItem)e.OriginalSource;
            var nodeItem = this.draggedOutConnectorItem.ParentNodeItem;
            this.draggedOutNodeDataContext = nodeItem.DataContext != null ? nodeItem.DataContext : nodeItem;
            this.draggedOutConnectorDataContext = this.draggedOutConnectorItem.DataContext != null ? this.draggedOutConnectorItem.DataContext : this.draggedOutConnectorItem;

            //
            // Raise an event so that application code can create a connection and
            // add it to the view-model.
            //
            ConnectionDragStartedEventArgs eventArgs = new ConnectionDragStartedEventArgs(ConnectionDragStartedEvent, this, this.draggedOutNodeDataContext, this.draggedOutConnectorDataContext);
            RaiseEvent(eventArgs);

            //
            // Retrieve the the view-model object for the connection was created by application code.
            //
            this.draggingConnectionDataContext = eventArgs.Connection;

            if (draggingConnectionDataContext == null)
            {
                //
                // Application code didn't create any connection.
                //
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// Event raised while the user is dragging a connector.
        /// </summary>
        private void ConnectorItem_Dragging(object source, ConnectorItemDraggingEventArgs e)
        {
            e.Handled = true;

            Trace.Assert((ConnectorItem)e.OriginalSource == this.draggedOutConnectorItem);

            //
            // Raise an event so that application code can compute intermediate connection points.
            //
            var connectionDraggingEventArgs =
                new ConnectionDraggingEventArgs(ConnectionDraggingEvent, this, 
                        this.draggedOutNodeDataContext, this.draggingConnectionDataContext, 
                        this.draggedOutConnectorDataContext);

            RaiseEvent(connectionDraggingEventArgs);
        }

        /// <summary>
        /// Event raised when the user has finished dragging a connector.
        /// </summary>
        private void ConnectorItem_DragCompleted(object source, ConnectorItemDragCompletedEventArgs e)
        {
            e.Handled = true;

            Trace.Assert((ConnectorItem)e.OriginalSource == this.draggedOutConnectorItem);

            Point mousePoint = Mouse.GetPosition(this);

            //
            // Figure out if the end of the connection was dropped on a connector.
            //
            NodeItem nodeDraggedOver = null;
            object nodeDataContextDraggedOver = null;
            DetermineConnectorItemDraggedOver(mousePoint, out nodeDraggedOver, out nodeDataContextDraggedOver);

            //
            // Raise an event to inform application code that connection dragging is complete.
            // The application code can determine if the connection between the two connectors
            // is valid and if so it is free to make the appropriate connection in the view-model.
            //
            RaiseEvent(new ConnectionDragCompletedEventArgs(ConnectionDragCompletedEvent, this, this.draggedOutNodeDataContext, this.draggingConnectionDataContext, this.draggedOutConnectorDataContext, nodeDataContextDraggedOver));

            this.IsDragging = false;
            this.IsNotDragging = true;
            this.IsDraggingConnection = false;
            this.IsNotDraggingConnection = true;
            this.draggedOutConnectorDataContext = null;
            this.draggedOutNodeDataContext = null;
            this.draggedOutConnectorItem = null;
            this.draggingConnectionDataContext = null;
        }

        /// <summary>
        /// This function does a hit test to determine which connector, if any, is under 'hitPoint'.
        /// </summary>
        private bool DetermineConnectorItemDraggedOver(Point hitPoint, out NodeItem nodeItemDraggedOver, out object nodeDataContextDraggedOver)
        {
            nodeItemDraggedOver = null;
            nodeDataContextDraggedOver = null;

            //
            // Run a hit test 
            //
            HitTestResult result = null;
            VisualTreeHelper.HitTest(nodeItemsControl, null, 
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

            //
            // Actually want a reference to a 'NodeItem'.  
            // The hit test may have hit a UI element that is below 'NodeItem' so
            // search up the tree.
            //
            var hitItem = result.VisualHit as FrameworkElement;
            if (hitItem == null)
            {
                return false;
            }
            var nodeItem = WpfUtils.FindVisualParentWithType<NodeItem>(hitItem);
			if (nodeItem == null)
            {
                return false;
            }

            var designerView = nodeItem.ParentDesignerView;
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

        #endregion Private Methods
    }
}
