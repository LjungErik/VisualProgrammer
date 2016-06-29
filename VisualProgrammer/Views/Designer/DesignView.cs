using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VisualProgrammer.Controls;
using VisualProgrammer.Factory.MouseActions;
using VisualProgrammer.Utilities;
using VisualProgrammer.ViewModels.Designer;
using VisualProgrammer.Views.Designer.Events;

namespace VisualProgrammer.Views.Designer
{
    public partial class DesignView : Control, IDropView
    {
        #region Private Data Members

        private NodeControl nodeControl = null;

        private ItemsControl connectionControl = null;

        /* Selection */
        private FrameworkElement selectionCanvas = null;

        private FrameworkElement selectionBorder = null;

        #endregion Private Data Members

        #region Dependency Properties

        public static readonly DependencyProperty NodesSourceProperty =
            DependencyProperty.Register("NodesSource", typeof(IEnumerable), typeof(DesignView));

        public static readonly DependencyProperty ConnectionsSourceProperty =
            DependencyProperty.Register("ConnectionsSource", typeof(IEnumerable), typeof(DesignView));

        public static readonly DependencyProperty MouseHandlerProperty =
            DependencyProperty.Register("MouseHandler", typeof(IMouseAction), typeof(DesignView));

        public static readonly RoutedEvent NodeDragStartedEvent =
            EventManager.RegisterRoutedEvent("NodeDrageStarted", RoutingStrategy.Bubble, typeof(NodeDragStartedEventHandler), typeof(DesignView));

        public static readonly RoutedEvent NodeDraggingEvent =
            EventManager.RegisterRoutedEvent("NodeDragging", RoutingStrategy.Bubble, typeof(NodeDraggingEventHandler), typeof(DesignView));

        public static readonly RoutedEvent NodeDragCompletedEvent =
            EventManager.RegisterRoutedEvent("NodeDragCompleted", RoutingStrategy.Bubble, typeof(NodeDragCompletedEventHandler), typeof(DesignView));

        public static readonly RoutedEvent ConnectionDragStartedEvent =
            EventManager.RegisterRoutedEvent("ConnectionDragStarted", RoutingStrategy.Bubble, typeof(ConnectionDragStartedEventHandler), typeof(DesignView));

        public static readonly RoutedEvent ConnectionDraggingEvent =
            EventManager.RegisterRoutedEvent("ConnectionDragging", RoutingStrategy.Bubble, typeof(ConnectionDraggingEventHandler), typeof(DesignView));

        public static readonly RoutedEvent ConnectionDragCompletedEvent =
            EventManager.RegisterRoutedEvent("ConnectionDragCompleted", RoutingStrategy.Bubble, typeof(ConnectionDragCompletedEventHandler), typeof(DesignView));

        public static readonly RoutedEvent DraggedOverEvent =
            EventManager.RegisterRoutedEvent("DraggedOver", RoutingStrategy.Bubble, typeof(DragDropEventHandler), typeof(DesignView));

        public static readonly RoutedEvent DroppedOverEvent =
            EventManager.RegisterRoutedEvent("DroppedOver", RoutingStrategy.Bubble, typeof(DragDropEventHandler), typeof(DesignView));

        #endregion Dependency Properties

        public DesignView()
        {
            this.Background = Brushes.White;

            AddHandler(Node.NodeDragStartedEvent, new NodeDragStartedEventHandler(Node_DragStarted));
            AddHandler(Node.NodeDraggingEvent, new NodeDraggingEventHandler(Node_Dragging));
            AddHandler(Node.NodeDragCompletedEvent, new NodeDragCompletedEventHandler(Node_DragCompleted));
            AddHandler(Connector.ConnectorDragStartedEvent, new ConnectorDragStartedEventHandler(Connector_DragStarted));
            AddHandler(Connector.ConnectorDraggingEvent, new ConnectorDraggingEventHander(Connector_Dragging));
            AddHandler(Connector.ConnectorDragCompletedEvent, new ConnectorDragCompletedEventHandler(Connector_DragCompleted));
        }

        public IEnumerable NodesSource
        {
            get
            {
                return (IEnumerable)GetValue(NodesSourceProperty);
            }
            set
            {
                SetValue(NodesSourceProperty, value);
            }
        }

        public IEnumerable ConnectionsSource
        {
            get
            {
                return (IEnumerable)GetValue(ConnectionsSourceProperty);
            }
            set
            {
                SetValue(ConnectionsSourceProperty, value);
            }
        }

        public IMouseAction MouseHandler
        {
            get
            {
                return (IMouseAction)GetValue(MouseHandlerProperty);
            }
            set
            {
                SetValue(MouseHandlerProperty, value);
            }
        }

        public event NodeDragStartedEventHandler NodeDragStarted
        {
            add { AddHandler(NodeDragStartedEvent, value); }
            remove { RemoveHandler(NodeDragStartedEvent, value); }
        }

        public event NodeDraggingEventHandler NodeDragging
        {
            add { AddHandler(NodeDraggingEvent, value); }
            remove { RemoveHandler(NodeDraggingEvent, value); }
        }

        public event NodeDragCompletedEventHandler NodeDragCompleted
        {
            add { AddHandler(NodeDragCompletedEvent, value); }
            remove { RemoveHandler(NodeDragCompletedEvent, value); }
        }

        public event ConnectionDragStartedEventHandler ConnectionDragStarted
        {
            add { AddHandler(ConnectionDragStartedEvent, value); }
            remove { RemoveHandler(ConnectionDragStartedEvent, value); }
        }

        public event ConnectionDraggingEventHandler ConnectionDragging
        {
            add { AddHandler(ConnectionDraggingEvent, value); }
            remove { RemoveHandler(ConnectionDraggingEvent, value); }
        }

        public event ConnectorDragCompletedEventHandler ConnectionDragCompleted
        {
            add { AddHandler(ConnectionDragCompletedEvent, value); }
            remove { RemoveHandler(ConnectionDragCompletedEvent, value); }
        }

        public event RoutedEventHandler DraggedOver
        {
            add { AddHandler(DraggedOverEvent, value); }
            remove { RemoveHandler(DraggedOverEvent, value); }
        }

        public event RoutedEventHandler DroppedOver
        {
            add { AddHandler(DroppedOverEvent, value); }
            remove { RemoveHandler(DroppedOverEvent, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.nodeControl = (NodeControl)this.Template.FindName("PART_NodesControl", this);
            if (this.nodeControl == null)
            {
                throw new ApplicationException("Failed to locate 'PART_NodesControl' in 'DesignView'.");
            }

            this.connectionControl = (ItemsControl)this.Template.FindName("PART_ConnectionsControl", this);
            if (this.connectionControl == null)
            {
                throw new ApplicationException("Failed to locate 'PART_ConnectionsControl' in 'DesignView'.");
            }

            this.selectionCanvas = (FrameworkElement)this.Template.FindName("PART_SelectionCanvas", this);
            if(this.selectionCanvas == null)
            {
                throw new ApplicationException("Failed to locate 'PART_SelectionCanvas' in 'DesignView'");
            }

            this.selectionBorder = (FrameworkElement)this.Template.FindName("PART_SelectionBorder", this);
            if(this.selectionBorder == null)
            {
                throw new ApplicationException("Failed to locate 'PART_SelectionBorder' in 'DesignView'");
            }
        }

        public int GetMaxIndex()
        {
            int maxIndex = 0;

            foreach(NodeViewModel node in NodesSource)
            {
                if (node.ZIndex > maxIndex)
                    maxIndex = node.ZIndex;
            }

            return maxIndex;
        }

        #region Selection Actions Methods

        public void InitSelectionArea(Point current, Point origin)
        {
            UpdateSelectionArea(current, origin);

            selectionCanvas.Visibility = Visibility.Visible;
        }

        public void UpdateSelectionArea(Point current, Point origin)
        {
            double x, y;
            double width, height;

            if (current.X > origin.X)
            {
                x = origin.X;
                width = current.X - origin.X;
            }
            else
            {
                x = current.X;
                width = origin.X - current.X;
            }

            if (current.Y > origin.Y)
            {
                y = origin.Y;
                height = current.Y - origin.Y;
            }
            else
            {
                y = current.Y;
                height = origin.Y - current.Y;
            }

            Canvas.SetLeft(selectionBorder, x);
            Canvas.SetTop(selectionBorder, y);
            selectionBorder.Width = width;
            selectionBorder.Height = height;
        }

        public void ApplySelection()
        {
            selectionCanvas.Visibility = Visibility.Collapsed;

            double x = Canvas.GetLeft(selectionBorder);
            double y = Canvas.GetTop(selectionBorder);
            double width = selectionBorder.Width;
            double height = selectionBorder.Height;
            Rect selectArea = new Rect(x, y, width, height);

            if(nodeControl.SelectedItems.Count > 0)
                nodeControl.SelectedItems.Clear();

            foreach(var nodeDataContext in NodesSource)
            {
                var node = (Node)nodeControl.ItemContainerGenerator.ContainerFromItem(nodeDataContext);
                var transformToAncestor = node.TransformToAncestor(this);
                Point itemPt1 = transformToAncestor.Transform(new Point(0, 0));
                Point itemPt2 = transformToAncestor.Transform(new Point(node.ActualWidth, node.ActualHeight));
                Rect itemRect = new Rect(itemPt1, itemPt2);
                if(selectArea.Contains(itemRect))
                {
                    node.IsSelected = true;
                }
            }
        }

        public void ClearSelected()
        {
            this.nodeControl.SelectedItems.Clear();
        }

        #endregion Selection Actions Methods

        #region Private Methods

        static DesignView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DesignView), new FrameworkPropertyMetadata(typeof(DesignView)));
        }

        #endregion Private Methods
    }
}
