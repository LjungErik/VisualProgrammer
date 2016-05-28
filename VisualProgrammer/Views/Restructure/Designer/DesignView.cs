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
using VisualProgrammer.Views.Restructure.Designer.Events;

namespace VisualProgrammer.Views.Restructure.Designer
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

        private static readonly DependencyPropertyKey NodesPropertyKey =
            DependencyProperty.RegisterReadOnly("Nodes", typeof(ObservableCollection<object>), typeof(DesignView),
                new FrameworkPropertyMetadata());
        public static readonly DependencyProperty NodesProperty = NodesPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey ConnectionsPropertyKey =
            DependencyProperty.RegisterReadOnly("Connections", typeof(ObservableCollection<object>), typeof(DesignView),
                new FrameworkPropertyMetadata());
        public static readonly DependencyProperty ConnectionsProperty = ConnectionsPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey TemperaryNodesPropertyKey =
            DependencyProperty.RegisterReadOnly("TemperaryNodes", typeof(ObservableCollection<object>), typeof(DesignView),
                new FrameworkPropertyMetadata());
        public static readonly DependencyProperty TemperaryNodesProperty = TemperaryNodesPropertyKey.DependencyProperty;

        public static readonly DependencyProperty NodesSourceProperty =
            DependencyProperty.Register("NodesSource", typeof(IEnumerable), typeof(DesignView),
                new FrameworkPropertyMetadata(NodesSource_PropertyChanged));

        public static readonly DependencyProperty ConnectionsSourceProperty =
            DependencyProperty.Register("ConnectionsSource", typeof(IEnumerable), typeof(DesignView),
                new FrameworkPropertyMetadata(ConnectionsSource_PropertyChanged));

        public static readonly DependencyProperty TemperaryNodesSourceProperty =
            DependencyProperty.Register("TemperaryNodesSource", typeof(IEnumerable), typeof(DesignView),
                new FrameworkPropertyMetadata(TemperaryNodesSource_PropertyChanged));

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
            this.Nodes = new ObservableCollection<object>();

            this.Connections = new ObservableCollection<object>();

            this.TemperaryNodes = new ObservableCollection<object>();

            this.Background = Brushes.White;

            AddHandler(Node.NodeDragStartedEvent, new NodeDragStartedEventHandler(Node_DragStarted));
            AddHandler(Node.NodeDraggingEvent, new NodeDraggingEventHandler(Node_Dragging));
            AddHandler(Node.NodeDragCompletedEvent, new NodeDragCompletedEventHandler(Node_DragCompleted));
            AddHandler(Connector.ConnectorDragStartedEvent, new ConnectorDragStartedEventHandler(Connector_DragStarted));
            AddHandler(Connector.ConnectorDraggingEvent, new ConnectorDraggingEventHander(Connector_Dragging));
            AddHandler(Connector.ConnectorDragCompletedEvent, new ConnectorDragCompletedEventHandler(Connector_DragCompleted));
        }

        public ObservableCollection<object> Nodes
        {
            get
            {
                return (ObservableCollection<object>)GetValue(NodesProperty);
            }
            private set
            {
                SetValue(NodesPropertyKey, value);
            }
        }

        public ObservableCollection<object> Connections
        {
            get
            {
                return (ObservableCollection<object>)GetValue(ConnectionsProperty);
            }
            private set
            {
                SetValue(ConnectionsPropertyKey, value);
            }
        }

        public ObservableCollection<object> TemperaryNodes
        {
            get
            {
                return (ObservableCollection<object>)GetValue(TemperaryNodesProperty);
            }
            set
            {
                SetValue(TemperaryNodesPropertyKey, value);
            }
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

        public IEnumerable TemperaryNodesSource
        {
            get
            {
                return (IEnumerable)GetValue(TemperaryNodesSourceProperty);
            }
            set
            {
                SetValue(TemperaryNodesSourceProperty, value);
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

            for(int nodeIndex = 0; nodeIndex < this.Nodes.Count; ++nodeIndex)
            {
                var node = (Node)nodeControl.ItemContainerGenerator.ContainerFromIndex(nodeIndex);
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

        #region Property Changed Methods

        private static void NodesSource_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DesignView c = (DesignView)d;

            c.Nodes.Clear();

            if (e.OldValue != null)
            {
                var notifyCollectionChanged = e.OldValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    notifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(c.NodesSource_CollectionChanged);
                }
            }

            if (e.NewValue != null)
            {
                var enumerable = e.NewValue as IEnumerable;
                if (enumerable != null)
                {
                    foreach (object obj in enumerable)
                    {
                        c.Nodes.Add(obj);
                    }
                }

                var notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    notifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(c.NodesSource_CollectionChanged);
                }
            }
        }

        private void NodesSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Nodes.Clear();
            }
            else
            {
                if (e.OldItems != null)
                {
                    foreach (object obj in e.OldItems)
                    {
                        Nodes.Remove(obj);
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (object obj in e.NewItems)
                    {
                        Nodes.Add(obj);
                    }
                }
            }
        }

        private static void ConnectionsSource_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DesignView c = (DesignView)d;

            c.Connections.Clear();

            if (e.OldValue != null)
            {
                INotifyCollectionChanged notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    notifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(c.ConnectionsSource_CollectionChanged);
                }
            }

            if (e.NewValue != null)
            {
                IEnumerable enumerable = e.NewValue as IEnumerable;
                if (enumerable != null)
                {
                    foreach (object obj in enumerable)
                    {
                        c.Connections.Add(obj);
                    }
                }

                INotifyCollectionChanged notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    notifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(c.ConnectionsSource_CollectionChanged);
                }
            }
        }

        private void ConnectionsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {

                Connections.Clear();
            }
            else
            {
                if (e.OldItems != null)
                {

                    foreach (object obj in e.OldItems)
                    {
                        Connections.Remove(obj);
                    }
                }

                if (e.NewItems != null)
                {

                    foreach (object obj in e.NewItems)
                    {
                        Connections.Add(obj);
                    }
                }
            }
        }

        private static void TemperaryNodesSource_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DesignView c = (DesignView)d;

            c.TemperaryNodes.Clear();

            if (e.OldValue != null)
            {
                INotifyCollectionChanged notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    notifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(c.TemperaryNodesSource_CollectionChanged);
                }
            }

            if (e.NewValue != null)
            {
                IEnumerable enumerable = e.NewValue as IEnumerable;
                if (enumerable != null)
                {
                    foreach (object obj in enumerable)
                    {
                        c.TemperaryNodes.Add(obj);
                    }
                }

                INotifyCollectionChanged notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    notifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(c.TemperaryNodesSource_CollectionChanged);
                }
            }
        }

        private void TemperaryNodesSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                TemperaryNodes.Clear();
            }
            else
            {
                if (e.OldItems != null)
                {
                    foreach (object obj in e.OldItems)
                    {
                        TemperaryNodes.Remove(obj);
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (object obj in e.NewItems)
                    {
                        TemperaryNodes.Add(obj);
                    }
                }
            }
        }

        #endregion Private Methods
    }
}
