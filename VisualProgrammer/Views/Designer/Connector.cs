using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VisualProgrammer.Views.Designer.Events;

namespace VisualProgrammer.Views.Designer
{
    public class Connector : ContentControl
    {
        #region Private Data Members

        private Point lastPosition;

        private bool isDragging = false;

        #endregion Private Data Members

        #region Dependency Properties/Events

        public static readonly DependencyProperty HotspotProperty =
            DependencyProperty.Register("Hotspot", typeof(Point), typeof(Connector));

        public static readonly DependencyProperty ParentNodeProperty =
            DependencyProperty.Register("ParentNode", typeof(Node), typeof(Connector));

        public static readonly DependencyProperty ParentDesignViewProperty =
            DependencyProperty.Register("ParentDesignView", typeof(DesignView), typeof(Connector),
                new FrameworkPropertyMetadata());

        public static readonly RoutedEvent ConnectorDragStartedEvent =
            EventManager.RegisterRoutedEvent("ConnectorDragStarted", RoutingStrategy.Bubble, typeof(ConnectorDragStartedEventHandler), typeof(Connector));

        public static readonly RoutedEvent ConnectorDraggingEvent =
            EventManager.RegisterRoutedEvent("ConnectorDragging", RoutingStrategy.Bubble, typeof(ConnectorDraggingEventHander), typeof(Connector));

        public static readonly RoutedEvent ConnectorDragCompletedEvent =
            EventManager.RegisterRoutedEvent("ConnectorDragCompleted", RoutingStrategy.Bubble, typeof(ConnectorDragCompletedEventHandler), typeof(Connector));

        #endregion Dependency Properties/Events

        public Connector()
        {
            this.LayoutUpdated += new EventHandler(Connector_LayoutUpdated);
        }

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

        public Node ParentNode
        {
            get
            {
                return (Node)GetValue(ParentNodeProperty);
            }
            set
            {
                SetValue(ParentNodeProperty, value);
            }
        }

        public DesignView ParentDesignView
        {
            get
            {
                return (DesignView)GetValue(ParentDesignViewProperty);
            }
            set
            {
                SetValue(ParentDesignViewProperty, value);
            }
        }

        #region Private Methods

        static Connector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Connector), new FrameworkPropertyMetadata(typeof(Connector)));
        }

        private static void ParentDesignView_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Connector c = (Connector)o;
            c.UpdateHotspot();
        }

        private void Connector_LayoutUpdated(object sender, EventArgs e)
        {
            UpdateHotspot();
        }

        private void UpdateHotspot()
        {
            if (ParentDesignView == null)
                return;

            if(!ParentDesignView.IsAncestorOf(this))
            {
                this.ParentDesignView = null;
                return;
            }

            var centerPoint = new Point(this.ActualWidth / 2, this.ActualHeight / 2);
            this.Hotspot = this.TransformToAncestor(this.ParentDesignView).Transform(centerPoint);
        }

        #region MouseEvents

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (this.ParentDesignView != null)
            {
                this.ParentDesignView.Focus();

                if (e.ChangedButton == MouseButton.Left)
                {
                    ConnectorDragStartedEventArgs eventArgs = new ConnectorDragStartedEventArgs(ConnectorDragStartedEvent, this);
                    RaiseEvent(eventArgs);

                    if (!eventArgs.Cancel)
                    {
                        isDragging = true;
                        lastPosition = e.GetPosition(ParentDesignView);
                        this.CaptureMouse();
                    }
                    e.Handled = true;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if(isDragging)
            {
                Point currentPosition = e.GetPosition(ParentDesignView);
                Vector offset = currentPosition - lastPosition;
                if(offset.X != 0.0 ||
                    offset.Y != 0.0)
                {
                    lastPosition = currentPosition;

                    RaiseEvent(new ConnectorDraggingEventArgs(ConnectorDraggingEvent, this, offset.X, offset.Y));
                }

                e.Handled = true;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if(e.ChangedButton == MouseButton.Left)
            {
                if(isDragging)
                {
                    RaiseEvent(new ConnectorDragCompletedEventArgs(ConnectorDragCompletedEvent, this));

                    this.ReleaseMouseCapture();

                    isDragging = false;
                }

                e.Handled = true;
            }
        }

        #endregion MouseEvents

        #endregion Private Methods
    }
}
