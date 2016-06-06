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
    public class Node : ListBoxItem
    {
        #region Private Data Member

        private Point lastPosition;

        private bool isDragging = false;

        #endregion Private Data Member

        #region Dependency Properties/Events

        public static DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(Node),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(Node),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static DependencyProperty ZIndexProperty =
            DependencyProperty.Register("ZIndex", typeof(int), typeof(Node),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static DependencyProperty IsVisibleProperty =
            DependencyProperty.Register("IsVisible", typeof(bool), typeof(Node),
                new FrameworkPropertyMetadata(true));

        public static DependencyProperty ParentDesignViewProperty =
            DependencyProperty.Register("ParentDesignView", typeof(DesignView), typeof(Node));

        public static readonly RoutedEvent NodeDragStartedEvent =
            EventManager.RegisterRoutedEvent("NodeDrageStarted", RoutingStrategy.Bubble, typeof(NodeDragStartedEventHandler), typeof(Node));

        public static readonly RoutedEvent NodeDraggingEvent =
            EventManager.RegisterRoutedEvent("NodeDragging", RoutingStrategy.Bubble, typeof(NodeDraggingEventHandler), typeof(Node));

        public static readonly RoutedEvent NodeDragCompletedEvent =
            EventManager.RegisterRoutedEvent("NodeDragCompleted", RoutingStrategy.Bubble, typeof(NodeDragCompletedEventHandler), typeof(Node));

        #endregion Dependency Properties/Events

        public Node()
        {
        }

        public double X
        {
            get
            {
                return (double)GetValue(XProperty);
            }
            set
            {
                SetValue(XProperty, value);
            }
        }

        public double Y
        {
            get
            {
                return (double)GetValue(YProperty);
            }
            set
            {
                SetValue(YProperty, value);
            }
        }

        public int ZIndex
        {
            get
            {
                return (int)GetValue(ZIndexProperty);
            }
            set
            {
                SetValue(ZIndexProperty, value);
            }
        }

        public bool IsVisible
        {
            get
            {
                return (bool)GetValue(IsVisibleProperty);
            }
            set
            {
                SetValue(IsVisibleProperty, value);
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

        public void BeginDrag(Point location)
        {
            PerformDragAction(location);
        }

        #region Private Methods

        static Node()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Node), new FrameworkPropertyMetadata(typeof(Node)));
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            HandleMouseDownEvent(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (isDragging)
            {
                Point currentPosition = e.GetPosition(ParentDesignView);

                Vector offset = currentPosition - lastPosition;
                if (offset.X != 0.0 ||
                   offset.Y != 0.0)
                {
                    lastPosition = currentPosition;

                    RaiseEvent(new NodeDraggingEventArgs(NodeDraggingEvent, this, new Node[] { this }, offset.X, offset.Y));
                }
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (isDragging)
            {
                RaiseEvent(new NodeDragCompletedEventArgs(NodeDragCompletedEvent, this, new Node[] { this }));

                this.ReleaseMouseCapture();

                isDragging = false;
            }

            e.Handled = true;
        }

        private void HandleMouseDownEvent(MouseEventArgs e)
        {
            if (ParentDesignView != null)
            {
                ParentDesignView.Focus();

                if (e.LeftButton == MouseButtonState.Pressed)
                {

                    PerformLeftClickAction(e.GetPosition(ParentDesignView));
                    e.Handled = true;
                }
            }
        }

        private void PerformLeftClickAction(Point location)
        {
            HandleLeftClick();

            HandleDragging(location);
        }

        private void PerformDragAction(Point location)
        {
            IsSelected = true;

            HandleDragging(location);
        }

        private void HandleDragging(Point location)
        {
            NodeDragStartedEventArgs eventArgs = new NodeDragStartedEventArgs(NodeDragStartedEvent, this, new Node[] { this });
            RaiseEvent(eventArgs);

            if (!eventArgs.Cancel)
            {
                isDragging = true;
                lastPosition = location;
                CaptureMouse();
            }
        }

        private void HandleLeftClick()
        {
            if((Keyboard.Modifiers & ModifierKeys.Control) == 0)
                ParentDesignView.ClearSelected();

            IsSelected = true;
        }

        #endregion Private Methods
    }
}
