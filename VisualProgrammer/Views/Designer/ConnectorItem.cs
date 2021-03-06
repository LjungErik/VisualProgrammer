﻿using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

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
   * Modified on April 4 2016, by Erik Ljung
   */

    /// <summary>
    /// This is the UI element for a connector.
    /// Each nodes has multiple connectors that are used to connect it to other nodes.
    /// </summary>
    public class ConnectorItem : ContentControl
    {
        #region Dependency Property/Event Definitions

        public static readonly DependencyProperty HotspotProperty =
            DependencyProperty.Register("Hotspot", typeof(Point), typeof(ConnectorItem));

        public static readonly DependencyProperty ParentDesignerViewProperty =
            DependencyProperty.Register("ParentDesignerView", typeof(DesignerView), typeof(ConnectorItem),
                new FrameworkPropertyMetadata(ParentDesignerView_PropertyChanged));

        public static readonly DependencyProperty ParentNodeItemProperty =
            DependencyProperty.Register("ParentNodeItem", typeof(NodeItem), typeof(ConnectorItem));

        public static readonly RoutedEvent ConnectorDragStartedEvent =
            EventManager.RegisterRoutedEvent("ConnectorDragStarted", RoutingStrategy.Bubble, typeof(ConnectorItemDragStartedEventHandler), typeof(ConnectorItem));

        public static readonly RoutedEvent ConnectorDraggingEvent =
            EventManager.RegisterRoutedEvent("ConnectorDragging", RoutingStrategy.Bubble, typeof(ConnectorItemDraggingEventHandler), typeof(ConnectorItem));

        public static readonly RoutedEvent ConnectorDragCompletedEvent =
            EventManager.RegisterRoutedEvent("ConnectorDragCompleted", RoutingStrategy.Bubble, typeof(ConnectorItemDragCompletedEventHandler), typeof(ConnectorItem));

        #endregion Dependency Property/Event Definitions

        #region Private Data Members

        /// <summary>
        /// The point the mouse was last at when dragging.
        /// </summary>
        private Point lastMousePoint;

        /// <summary>
        /// Set to 'true' when left mouse button is held down.
        /// </summary>
        private bool isLeftMouseDown = false;

        /// <summary>
        /// Set to 'true' when the user is dragging the connector.
        /// </summary>
        private bool isDragging = false;

        /// <summary>
        /// The threshold distance the mouse-cursor must move before dragging begins.
        /// </summary>
        private static readonly double DragThreshold = 2;

        #endregion Private Data Members

        public ConnectorItem()
        {
            Focusable = false;

            this.LayoutUpdated += new EventHandler(ConnectorItem_LayoutUpdated);
        }

        /// <summary>
        /// Automatically updated dependency property that specifies the hotspot (or center point) of the connector.
        /// Specified in content coordinate.
        /// </summary>
        public Point Hotspot
        {
            get
            {
                return (Point)GetValue(HotspotProperty);
            }
            set
            {
                SetValue(HotspotProperty, value);
            }
        }

        #region Private Data Members\Properties

        /// <summary>
        /// Reference to the data-bound parent DesignerView.
        /// </summary>
        public DesignerView ParentDesignerView
        {
            get
            {
                return (DesignerView)GetValue(ParentDesignerViewProperty);
            }
            set
            {
                SetValue(ParentDesignerViewProperty, value);
            }
        }

       
        /// <summary>
        /// Reference to the data-bound parent NodeItem.
        /// </summary>
        public NodeItem ParentNodeItem
        {
            get
            {
                return (NodeItem)GetValue(ParentNodeItemProperty);
            }
            set
            {
                SetValue(ParentNodeItemProperty, value);
            }
        }

        #endregion Private Data Members\Properties

        #region Private Methods

        /// <summary>
        /// Static constructor.
        /// </summary>
        static ConnectorItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ConnectorItem), new FrameworkPropertyMetadata(typeof(ConnectorItem)));
        }

        /// <summary>
        /// A mouse button has been held down.
        /// </summary>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (this.ParentNodeItem != null)
            {
                this.ParentNodeItem.BringToFront();
            }

            if (this.ParentDesignerView != null)
            {
                this.ParentDesignerView.Focus();
            }

            if (e.ChangedButton == MouseButton.Left)
            {
                if (this.ParentNodeItem != null)
                {
                    //
                    // Delegate to parent node to execute selection logic.
                    //
                    this.ParentNodeItem.LeftMouseDownSelectionLogic();
                }

                lastMousePoint = e.GetPosition(this.ParentDesignerView);
                isLeftMouseDown = true;
                e.Handled = true;
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                if (this.ParentNodeItem != null)
                {
                    //
                    // Delegate to parent node to execute selection logic.
                    //
                    this.ParentNodeItem.RightMouseDownSelectionLogic();
                }
            }
        }

        /// <summary>
        /// The mouse cursor has been moved.
        /// </summary>        
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (isDragging)
            {
                //
                // Raise the event to notify that dragging is in progress.
                //

                Point curMousePoint = e.GetPosition(this.ParentDesignerView);
                Vector offset = curMousePoint - lastMousePoint;
                if (offset.X != 0.0 ||
                    offset.Y != 0.0)
                {
                    lastMousePoint = curMousePoint;

                    RaiseEvent(new ConnectorItemDraggingEventArgs(ConnectorDraggingEvent, this, offset.X, offset.Y));
                }

                e.Handled = true;
            }
            else if (isLeftMouseDown)
            {
                if (this.ParentDesignerView != null &&
                    this.ParentDesignerView.EnableConnectionDragging)
                {
                    //
                    // The user is left-dragging the connector and connection dragging is enabled,
                    // but don't initiate the drag operation until 
                    // the mouse cursor has moved more than the threshold distance.
                    //
                    Point curMousePoint = e.GetPosition(this.ParentDesignerView);
                    var dragDelta = curMousePoint - lastMousePoint;
                    double dragDistance = Math.Abs(dragDelta.Length);
                    if (dragDistance > DragThreshold)
                    {
                        var eventArgs = new ConnectorItemDragStartedEventArgs(ConnectorDragStartedEvent, this);
                        RaiseEvent(eventArgs);

                        if (eventArgs.Cancel)
                        {
                            //
                            // Handler of the event disallowed dragging of the node.
                            //
                            isLeftMouseDown = false;
                            return;
                        }

                        isDragging = true;
                        this.CaptureMouse();
                        e.Handled = true;
                    }
                }
            }
        }

        /// <summary>
        /// A mouse button has been released.
        /// </summary>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.ChangedButton == MouseButton.Left)
            {
                if (isLeftMouseDown)
                {
                    if (isDragging)
                    {
                        RaiseEvent(new ConnectorItemDragCompletedEventArgs(ConnectorDragCompletedEvent, this));
                        
                        this.ReleaseMouseCapture();

                        isDragging = false;
                    }
                    else
                    {
                        //
                        // Execute mouse up selection logic only if there was no drag operation.
                        //
                        if (this.ParentNodeItem != null)
                        {
                            //
                            // Delegate to parent node to execute selection logic.
                            //
                            this.ParentNodeItem.LeftMouseUpSelectionLogic();
                        }
                    }

                    isLeftMouseDown = false;

                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Cancel connection dragging for the connector that was dragged out.
        /// </summary>
        internal void CancelConnectionDragging()
        {
            if (isLeftMouseDown)
            {
                //
                // Raise ConnectorDragCompleted, with a null connector.
                //
                RaiseEvent(new ConnectorItemDragCompletedEventArgs(ConnectorDragCompletedEvent, null));

                isLeftMouseDown = false;
                this.ReleaseMouseCapture();
            }
        }

        /// <summary>
        /// Event raised when 'ParentDesignerView' property has changed.
        /// </summary>
        private static void ParentDesignerView_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectorItem c = (ConnectorItem)d;
            c.UpdateHotspot();
        }

        /// <summary>
        /// Event raised when the layout of the connector has been updated.
        /// </summary>
        private void ConnectorItem_LayoutUpdated(object sender, EventArgs e)
        {
            UpdateHotspot();
        }

        /// <summary>
        /// Update the connector hotspot.
        /// </summary>
        private void UpdateHotspot()
        {
            if (this.ParentDesignerView == null)
            {
                // No parent DesignerView is set.
                return;
            }

            if (!this.ParentDesignerView.IsAncestorOf(this))
            {
                this.ParentDesignerView = null;
                return;
            }

            var centerPoint = new Point(this.ActualWidth / 2, this.ActualHeight / 2);

            //
            // Transform the center point so that it is relative to the parent DesignerView.
            // Then assign it to Hotspot.  Usually Hotspot will be data-bound to the application
            // view-model using OneWayToSource so that the value of the hotspot is then pushed through
            // to the view-model.
            //
            this.Hotspot = this.TransformToAncestor(this.ParentDesignerView).Transform(centerPoint);
       }

        #endregion Private Methods
    }
}
