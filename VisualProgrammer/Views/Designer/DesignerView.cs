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
   * Modified on April 4 2016, by Erik Ljung
   */

    /// <summary>
    /// The main class that implements the Designer/flow-chart control.
    /// </summary>
    public partial class DesignerView : Control
    {
        #region Dependency Property/Event Definitions

        private static readonly DependencyPropertyKey NodesPropertyKey =
            DependencyProperty.RegisterReadOnly("Nodes", typeof(ImpObservableCollection<object>), typeof(DesignerView),
                new FrameworkPropertyMetadata());
        public static readonly DependencyProperty NodesProperty = NodesPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey ConnectionsPropertyKey =
            DependencyProperty.RegisterReadOnly("Connections", typeof(ImpObservableCollection<object>), typeof(DesignerView),
                new FrameworkPropertyMetadata());
        public static readonly DependencyProperty ConnectionsProperty = ConnectionsPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey TemperaryNodesPropertyKey =
            DependencyProperty.RegisterReadOnly("TemperaryNodes", typeof(ImpObservableCollection<object>), typeof(DesignerView),
                new FrameworkPropertyMetadata());
        public static readonly DependencyProperty TemperaryNodesProperty = TemperaryNodesPropertyKey.DependencyProperty;

        public static readonly DependencyProperty NodesSourceProperty =
            DependencyProperty.Register("NodesSource", typeof(IEnumerable), typeof(DesignerView),
                new FrameworkPropertyMetadata(NodesSource_PropertyChanged));

        public static readonly DependencyProperty ConnectionsSourceProperty =
            DependencyProperty.Register("ConnectionsSource", typeof(IEnumerable), typeof(DesignerView),
                new FrameworkPropertyMetadata(ConnectionsSource_PropertyChanged));

        public static readonly DependencyProperty TemperaryNodesSourceProperty =
            DependencyProperty.Register("TemperaryNodesSource", typeof(IEnumerable), typeof(DesignerView),
                new FrameworkPropertyMetadata(TemperaryNodesSource_PropertyChanged));

        public static readonly DependencyProperty IsClearSelectionOnEmptySpaceClickEnabledProperty =
            DependencyProperty.Register("IsClearSelectionOnEmptySpaceClickEnabled", typeof(bool), typeof(DesignerView),
                new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty EnableConnectionDraggingProperty =
            DependencyProperty.Register("EnableConnectionDragging", typeof(bool), typeof(DesignerView),
                new FrameworkPropertyMetadata(true));

        private static readonly DependencyPropertyKey IsDraggingConnectionPropertyKey =
            DependencyProperty.RegisterReadOnly("IsDraggingConnection", typeof(bool), typeof(DesignerView),
                new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsDraggingConnectionProperty = IsDraggingConnectionPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsNotDraggingConnectionPropertyKey =
            DependencyProperty.RegisterReadOnly("IsNotDraggingConnection", typeof(bool), typeof(DesignerView),
                new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty IsNotDraggingConnectionProperty = IsNotDraggingConnectionPropertyKey.DependencyProperty;

        public static readonly DependencyProperty EnableNodeDraggingProperty =
            DependencyProperty.Register("EnableNodeDragging", typeof(bool), typeof(DesignerView),
                new FrameworkPropertyMetadata(true));

        private static readonly DependencyPropertyKey IsDraggingNodePropertyKey =
            DependencyProperty.RegisterReadOnly("IsDraggingNode", typeof(bool), typeof(DesignerView),
                new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsDraggingNodeProperty = IsDraggingNodePropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsNotDraggingNodePropertyKey =
            DependencyProperty.RegisterReadOnly("IsNotDraggingNode", typeof(bool), typeof(DesignerView),
                new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty IsNotDraggingNodeProperty = IsDraggingNodePropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsDraggingPropertyKey =
            DependencyProperty.RegisterReadOnly("IsDragging", typeof(bool), typeof(DesignerView),
                new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsDraggingProperty = IsDraggingPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsNotDraggingPropertyKey =
            DependencyProperty.RegisterReadOnly("IsNotDragging", typeof(bool), typeof(DesignerView),
                new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty IsNotDraggingProperty = IsNotDraggingPropertyKey.DependencyProperty;
 
        public static readonly DependencyProperty NodeItemTemplateProperty =
            DependencyProperty.Register("NodeItemTemplate", typeof(DataTemplate), typeof(DesignerView));

        public static readonly DependencyProperty NodeItemTemplateSelectorProperty =
            DependencyProperty.Register("NodeItemTemplateSelector", typeof(DataTemplateSelector), typeof(DesignerView));

        public static readonly DependencyProperty NodeItemContainerStyleProperty =
            DependencyProperty.Register("NodeItemContainerStyle", typeof(Style), typeof(DesignerView));

        public static readonly DependencyProperty ConnectionItemTemplateProperty =
            DependencyProperty.Register("ConnectionItemTemplate", typeof(DataTemplate), typeof(DesignerView));

        public static readonly DependencyProperty ConnectionItemTemplateSelectorProperty =
            DependencyProperty.Register("ConnectionItemTemplateSelector", typeof(DataTemplateSelector), typeof(DesignerView));

        public static readonly DependencyProperty ConnectionItemContainerStyleProperty =
            DependencyProperty.Register("ConnectionItemContainerStyle", typeof(Style), typeof(DesignerView));

        public static readonly RoutedEvent NodeDragStartedEvent =
            EventManager.RegisterRoutedEvent("NodeDragStarted", RoutingStrategy.Bubble, typeof(NodeDragStartedEventHandler), typeof(DesignerView));

        public static readonly RoutedEvent NodeDraggingEvent =
            EventManager.RegisterRoutedEvent("NodeDragging", RoutingStrategy.Bubble, typeof(NodeDraggingEventHandler), typeof(DesignerView));

        public static readonly RoutedEvent NodeDragCompletedEvent =
            EventManager.RegisterRoutedEvent("NodeDragCompleted", RoutingStrategy.Bubble, typeof(NodeDragCompletedEventHandler), typeof(DesignerView));

        public static readonly RoutedEvent ConnectionDragStartedEvent =
            EventManager.RegisterRoutedEvent("ConnectionDragStarted", RoutingStrategy.Bubble, typeof(ConnectionDragStartedEventHandler), typeof(DesignerView));

        public static readonly RoutedEvent ConnectionDraggingEvent =
            EventManager.RegisterRoutedEvent("ConnectionDragging", RoutingStrategy.Bubble, typeof(ConnectionDraggingEventHandler), typeof(DesignerView));

        public static readonly RoutedEvent ConnectionDragCompletedEvent =
            EventManager.RegisterRoutedEvent("ConnectionDragCompleted", RoutingStrategy.Bubble, typeof(ConnectionDragCompletedEventHandler), typeof(DesignerView));

        public static readonly RoutedCommand SelectAllCommand = null;
        public static readonly RoutedCommand SelectNoneCommand = null;
        public static readonly RoutedCommand InvertSelectionCommand = null;
        public static readonly RoutedCommand CancelConnectionDraggingCommand = null;

        #endregion Dependency Property/Event Definitions

        #region Private Data Members

        /// <summary>
        /// Cached reference to the NodeItemsControl in the visual-tree.
        /// </summary>
        private NodeItemsControl nodeItemsControl = null;

        /// <summary>
        /// Cached reference to the ItemsControl for connections in the visual-tree.
        /// </summary>
        private ItemsControl connectionItemsControl = null;

        /// <summary>
        /// Cached list of currently selected nodes.
        /// </summary>
        private List<object> initialSelectedNodes = null;

        #endregion Private Data Members

        public DesignerView()
        {
            //
            // Create a collection to contain nodes.
            //
            this.Nodes = new ImpObservableCollection<object>();

            //
            // Create a collection to contain connections.
            //
            this.Connections = new ImpObservableCollection<object>();

            //
            // Create a collection to contain temperary nodes.
            //
            this.TemperaryNodes = new ImpObservableCollection<object>();

            //
            // Default background is white.
            //
            this.Background = Brushes.White;

            //
            // Add handlers for node and connector drag events.
            //
            AddHandler(NodeItem.NodeDragStartedEvent, new NodeDragStartedEventHandler(NodeItem_DragStarted));
            AddHandler(NodeItem.NodeDraggingEvent, new NodeDraggingEventHandler(NodeItem_Dragging));
            AddHandler(NodeItem.NodeDragCompletedEvent, new NodeDragCompletedEventHandler(NodeItem_DragCompleted));
            AddHandler(ConnectorItem.ConnectorDragStartedEvent, new ConnectorItemDragStartedEventHandler(ConnectorItem_DragStarted));
            AddHandler(ConnectorItem.ConnectorDraggingEvent, new ConnectorItemDraggingEventHandler(ConnectorItem_Dragging));
            AddHandler(ConnectorItem.ConnectorDragCompletedEvent, new ConnectorItemDragCompletedEventHandler(ConnectorItem_DragCompleted));
        }

        /// <summary>
        /// Event raised when the user starts dragging a node in the Designer.
        /// </summary>
        public event NodeDragStartedEventHandler NodeDragStarted
        {
            add { AddHandler(NodeDragStartedEvent, value); }
            remove { RemoveHandler(NodeDragStartedEvent, value); }
        }

        /// <summary>
        /// Event raised while user is dragging a node in the Designer.
        /// </summary>
        public event NodeDraggingEventHandler NodeDragging
        {
            add { AddHandler(NodeDraggingEvent, value); }
            remove { RemoveHandler(NodeDraggingEvent, value); }
        }

        /// <summary>
        /// Event raised when the user has completed dragging a node in the designer.
        /// </summary>
        public event NodeDragCompletedEventHandler NodeDragCompleted
        {
            add { AddHandler(NodeDragCompletedEvent, value); }
            remove { RemoveHandler(NodeDragCompletedEvent, value); }
        }

        /// <summary>
        /// Event raised when the user starts dragging a connector in the designer.
        /// </summary>
        public event ConnectionDragStartedEventHandler ConnectionDragStarted
        {
            add { AddHandler(ConnectionDragStartedEvent, value); }
            remove { RemoveHandler(ConnectionDragStartedEvent, value); }
        }

        /// <summary>
        /// Event raised when a connection is being dragged.
        /// </summary>
        public event ConnectionDraggingEventHandler ConnectionDragging
        {
            add { AddHandler(ConnectionDraggingEvent, value); }
            remove { RemoveHandler(ConnectionDraggingEvent, value); }
        }

        /// <summary>
        /// Event raised when the user has completed dragging a connection in the designer.
        /// </summary>
        public event ConnectionDragCompletedEventHandler ConnectionDragCompleted
        {
            add { AddHandler(ConnectionDragCompletedEvent, value); }
            remove { RemoveHandler(ConnectionDragCompletedEvent, value); }
        }

        /// <summary>
        /// Collection of nodes in the designer.
        /// </summary>
        public ImpObservableCollection<object> Nodes
        {
            get
            {
                return (ImpObservableCollection<object>)GetValue(NodesProperty);
            }
            private set
            {
                SetValue(NodesPropertyKey, value);
            }
        }

        /// <summary>
        /// Collection of connections in the designer.
        /// </summary>
        public ImpObservableCollection<object> Connections
        {
            get
            {
                return (ImpObservableCollection<object>)GetValue(ConnectionsProperty);
            }
            private set
            {
                SetValue(ConnectionsPropertyKey, value);
            }
        }

        /// <summary>
        /// Collection of temperary created nodes (storage for adding them to Nodes later)
        /// </summary>
        public ImpObservableCollection<object> TemperaryNodes
        {
            get
            {
                return (ImpObservableCollection<object>)GetValue(TemperaryNodesProperty);
            }
            set
            {
                SetValue(TemperaryNodesPropertyKey, value);
            }
        }

        /// <summary>
        /// A reference to the collection that is the source used to populate 'Nodes'.
        /// Used in the same way as 'ItemsSource' in 'ItemsControl'.
        /// </summary>
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

        /// <summary>
        /// A reference to the collection that is the source used to populate 'Connections'.
        /// Used in the same way as 'ItemsSource' in 'ItemsControl'.
        /// </summary>
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

        /// <summary>
        /// A reference to the collection that is the source used to populate 'TemperaryNodes'.
        /// Used in the same way as 'ItemsSource' in 'ItemsControl'.
        /// </summary>
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

        /// <summary>
        /// Set to 'true' to enable the clearing of selection when empty space is clicked.
        /// This is set to 'true' by default.
        /// </summary>
        public bool IsClearSelectionOnEmptySpaceClickEnabled
        {
            get
            {
                return (bool) GetValue(IsClearSelectionOnEmptySpaceClickEnabledProperty);
            }
            set
            {
                SetValue(IsClearSelectionOnEmptySpaceClickEnabledProperty, value);
            }
        }

        /// <summary>
        /// Set to 'true' to enable drag out of connectors to create new connections.
        /// </summary>
        public bool EnableConnectionDragging
        {
            get
            {
                return (bool)GetValue(EnableConnectionDraggingProperty);
            }
            set
            {
                SetValue(EnableConnectionDraggingProperty, value);
            }
        }

        /// <summary>
        /// Dependency property that is set to 'true' when the user is 
        /// dragging out a connection.
        /// </summary>
        public bool IsDraggingConnection
        {
            get
            {
                return (bool)GetValue(IsDraggingConnectionProperty);
            }
            private set
            {
                SetValue(IsDraggingConnectionPropertyKey, value);
            }
        }

        /// <summary>
        /// Dependency property that is set to 'false' when the user is 
        /// dragging out a connection.
        /// </summary>
        public bool IsNotDraggingConnection
        {
            get
            {
                return (bool)GetValue(IsNotDraggingConnectionProperty);
            }
            private set
            {
                SetValue(IsNotDraggingConnectionPropertyKey, value);
            }
        }

        /// <summary>
        /// Set to 'true' to enable dragging of nodes.
        /// </summary>
        public bool EnableNodeDragging
        {
            get
            {
                return (bool)GetValue(EnableNodeDraggingProperty);
            }
            set
            {
                SetValue(EnableNodeDraggingProperty, value);
            }
        }

        /// <summary>
        /// Dependency property that is set to 'true' when the user is 
        /// dragging out a connection.
        /// </summary>
        public bool IsDraggingNode
        {
            get
            {
                return (bool)GetValue(IsDraggingNodeProperty);
            }
            private set
            {
                SetValue(IsDraggingNodePropertyKey, value);
            }
        }

        /// <summary>
        /// Dependency property that is set to 'false' when the user is 
        /// dragging out a connection.
        /// </summary>
        public bool IsNotDraggingNode
        {
            get
            {
                return (bool)GetValue(IsNotDraggingNodeProperty);
            }
            private set
            {
                SetValue(IsNotDraggingNodePropertyKey, value);
            }
        }

        /// <summary>
        /// Set to 'true' when the user is dragging either a node or a connection.
        /// </summary>
        public bool IsDragging
        {
            get
            {
                return (bool)GetValue(IsDraggingProperty);
            }
            private set
            {
                SetValue(IsDraggingPropertyKey, value);
            }
        }

        /// <summary>
        /// Set to 'true' when the user is not dragging anything.
        /// </summary>
        public bool IsNotDragging
        {
            get
            {
                return (bool)GetValue(IsNotDraggingProperty);
            }
            private set
            {
                SetValue(IsNotDraggingPropertyKey, value);
            }
        }

        /// <summary>
        /// Gets or sets the DataTemplate used to display each node item.
        /// This is the equivalent to 'ItemTemplate' for ItemsControl.
        /// </summary>
        public DataTemplate NodeItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(NodeItemTemplateProperty);
            }
            set
            {
                SetValue(NodeItemTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets custom style-selection logic for a style that can be applied to each generated container element. 
        /// This is the equivalent to 'ItemTemplateSelector' for ItemsControl.
        /// </summary>
        public DataTemplateSelector NodeItemTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(NodeItemTemplateSelectorProperty);
            }
            set
            {
                SetValue(NodeItemTemplateSelectorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Style that is applied to the item container for each node item.
        /// This is the equivalent to 'ItemContainerStyle' for ItemsControl.
        /// </summary>
        public Style NodeItemContainerStyle
        {
            get
            {
                return (Style)GetValue(NodeItemContainerStyleProperty);
            }
            set
            {
                SetValue(NodeItemContainerStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the DataTemplate used to display each connection item.
        /// This is the equivalent to 'ItemTemplate' for ItemsControl.
        /// </summary>
        public DataTemplate ConnectionItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ConnectionItemTemplateProperty);
            }
            set
            {
                SetValue(ConnectionItemTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets custom style-selection logic for a style that can be applied to each generated container element. 
        /// This is the equivalent to 'ItemTemplateSelector' for ItemsControl.
        /// </summary>
        public DataTemplateSelector ConnectionItemTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(ConnectionItemTemplateSelectorProperty);
            }
            set
            {
                SetValue(ConnectionItemTemplateSelectorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Style that is applied to the item container for each connection item.
        /// This is the equivalent to 'ItemContainerStyle' for ItemsControl.
        /// </summary>
        public Style ConnectionItemContainerStyle
        {
            get
            {
                return (Style)GetValue(ConnectionItemContainerStyleProperty);
            }
            set
            {
                SetValue(ConnectionItemContainerStyleProperty, value);
            }
        }

        /// <summary>
        /// A reference to currently selected node.
        /// </summary>
        public object SelectedNode
        {
            get
            {
                if (nodeItemsControl != null)
                {
                    return nodeItemsControl.SelectedItem;
                }
                else
                {
                    if (initialSelectedNodes == null)
                    {
                        return null;
                    }

                    if (initialSelectedNodes.Count != 1)
                    {
                        return null;
                    }

                    return initialSelectedNodes[0];
                }
            }
            set
            {
                if (nodeItemsControl != null)
                {
                    nodeItemsControl.SelectedItem = value;
                }
                else
                {
                    if (initialSelectedNodes == null)
                    {
                        initialSelectedNodes = new List<object>();
                    }

                    initialSelectedNodes.Clear();
                    initialSelectedNodes.Add(value);
                }
            }
        }

        /// <summary>
        /// A list of selected nodes.
        /// </summary>
        public IList SelectedNodes
        {
            get
            {
                if (nodeItemsControl != null)
                {
                    return nodeItemsControl.SelectedItems;
                }
                else
                {
                    if (initialSelectedNodes == null)
                    {
                        initialSelectedNodes = new List<object>();
                    }

                    return initialSelectedNodes;
                }
            }
        }

        /// <summary>
        /// An event raised when the nodes selected in the DesignerView has changed.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;

        /// <summary>
        /// Bring the currently selected nodes into view.
        /// This affects ContentViewportOffsetX/ContentViewportOffsetY, but doesn't affect 'ContentScale'.
        /// </summary>
        public void BringSelectedNodesIntoView()
        {
            BringNodesIntoView(SelectedNodes);
        }

        /// <summary>
        /// Bring the collection of nodes into view.
        /// This affects ContentViewportOffsetX/ContentViewportOffsetY, but doesn't affect 'ContentScale'.
        /// </summary>
        public void BringNodesIntoView(ICollection nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException("'nodes' argument shouldn't be null.");
            }

            if (nodes.Count == 0)
            {
                return;
            }

            Rect rect = Rect.Empty;

            foreach (var node in nodes)
            {
                NodeItem nodeItem = FindAssociatedNodeItem(node);
                Rect nodeRect = new Rect(nodeItem.X, nodeItem.Y, nodeItem.ActualWidth, nodeItem.ActualHeight);

                if (rect == Rect.Empty)
                {
                    rect = nodeRect;
                }
                else
                {
                    rect.Intersect(nodeRect);
                }
            }

            this.BringIntoView(rect);
        }

        /// <summary>
        /// Clear the selection.
        /// </summary>
        public void SelectNone()
        {
            this.SelectedNodes.Clear();
        }

        /// <summary>
        /// Selects all of the nodes.
        /// </summary>
        public void SelectAll()
        {
            if (this.SelectedNodes.Count != this.Nodes.Count)
            {
                this.SelectedNodes.Clear();
                foreach (var node in this.Nodes)
                {
                    this.SelectedNodes.Add(node);
                }
            }
        }

        /// <summary>
        /// Inverts the current selection.
        /// </summary>
        public void InvertSelection()
        {
            var selectedNodesCopy = new ArrayList(this.SelectedNodes);
            this.SelectedNodes.Clear();

            foreach (var node in this.Nodes)
            {
                if (!selectedNodesCopy.Contains(node))
                {
                    this.SelectedNodes.Add(node);
                }
            }
        }

        /// <summary>
        /// When connection dragging is progress this function cancels it.
        /// </summary>
        public void CancelConnectionDragging()
        {
            if (!this.IsDraggingConnection)
            {
                return;
            }

            draggedOutConnectorItem.CancelConnectionDragging();

            this.IsDragging = false;
            this.IsNotDragging = true;
            this.IsDraggingConnection = false;
            this.IsNotDraggingConnection = true;
            this.draggedOutConnectorItem = null;
            this.draggedOutNodeDataContext = null;
            this.draggedOutConnectorDataContext = null;
            this.draggingConnectionDataContext = null;
        }

        #region Private Methods

        /// <summary>
        /// Static constructor.
        /// </summary>
        static DesignerView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DesignerView), new FrameworkPropertyMetadata(typeof(DesignerView)));

            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.A, ModifierKeys.Control));
            SelectAllCommand = new RoutedCommand("SelectAll", typeof(DesignerView), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.Escape));
            SelectNoneCommand = new RoutedCommand("SelectNone", typeof(DesignerView), inputs);

            inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.I, ModifierKeys.Control));
            InvertSelectionCommand = new RoutedCommand("InvertSelection", typeof(DesignerView), inputs);

            CancelConnectionDraggingCommand = new RoutedCommand("CancelConnectionDragging", typeof(DesignerView));

            CommandBinding binding = new CommandBinding();
            binding.Command = SelectAllCommand;
            binding.Executed += new ExecutedRoutedEventHandler(SelectAll_Executed);
            CommandManager.RegisterClassCommandBinding(typeof(DesignerView), binding);

            binding = new CommandBinding();
            binding.Command = SelectNoneCommand;
            binding.Executed += new ExecutedRoutedEventHandler(SelectNone_Executed);
            CommandManager.RegisterClassCommandBinding(typeof(DesignerView), binding);

            binding = new CommandBinding();
            binding.Command = InvertSelectionCommand;
            binding.Executed += new ExecutedRoutedEventHandler(InvertSelection_Executed);
            CommandManager.RegisterClassCommandBinding(typeof(DesignerView), binding);

            binding = new CommandBinding();
            binding.Command = CancelConnectionDraggingCommand;
            binding.Executed += new ExecutedRoutedEventHandler(CancelConnectionDragging_Executed);
            CommandManager.RegisterClassCommandBinding(typeof(DesignerView), binding);
        }

        /// <summary>
        /// Executes the 'SelectAll' command.
        /// </summary>
        private static void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DesignerView c = (DesignerView)sender;
            c.SelectAll();
        }

        /// <summary>
        /// Executes the 'SelectNone' command.
        /// </summary>
        private static void SelectNone_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DesignerView c = (DesignerView)sender;
            c.SelectNone();
        }

        /// <summary>
        /// Executes the 'InvertSelection' command.
        /// </summary>
        private static void InvertSelection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DesignerView c = (DesignerView)sender;
            c.InvertSelection();
        }

        /// <summary>
        /// Executes the 'CancelConnectionDragging' command.
        /// </summary>
        private static void CancelConnectionDragging_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DesignerView c = (DesignerView)sender;
            c.CancelConnectionDragging();
        }

        /// <summary>
        /// Event raised when a new collection has been assigned to the 'NodesSource' property.
        /// </summary>
        private static void NodesSource_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DesignerView c = (DesignerView)d;

            //
            // Clear 'Nodes'.
            //
            c.Nodes.Clear();

            if (e.OldValue != null)
            {
                var notifyCollectionChanged = e.OldValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    //
                    // Unhook events from previous collection.
                    //
                    notifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(c.NodesSource_CollectionChanged);
                }
            }

            if (e.NewValue != null)
            {
                var enumerable = e.NewValue as IEnumerable;
                if (enumerable != null)
                {
                    //
                    // Populate 'Nodes' from 'NodesSource'.
                    //
                    foreach (object obj in enumerable)
                    {
                        c.Nodes.Add(obj);
                    }
                }

                var notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    //
                    // Hook events in new collection.
                    //
                    notifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(c.NodesSource_CollectionChanged);
                }
            }
        }

        /// <summary>
        /// Event raised when a node has been added to or removed from the collection assigned to 'NodesSource'.
        /// </summary>
        private void NodesSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                //
                // 'NodesSource' has been cleared, also clear 'Nodes'.
                //
                Nodes.Clear();
            }
            else
            {
                if (e.OldItems != null)
                {
                    //
                    // For each item that has been removed from 'NodesSource' also remove it from 'Nodes'.
                    //
                    foreach (object obj in e.OldItems)
                    {
                        Nodes.Remove(obj);
                    }
                }

                if (e.NewItems != null)
                {
                    //
                    // For each item that has been added to 'NodesSource' also add it to 'Nodes'.
                    //
                    foreach (object obj in e.NewItems)
                    {
                        Nodes.Add(obj);
                    }
                }
            }
        }

        /// <summary>
        /// Event raised when a new collection has been assigned to the 'ConnectionsSource' property.
        /// </summary>
        private static void ConnectionsSource_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DesignerView c = (DesignerView)d;

            //
            // Clear 'Connections'.
            //
            c.Connections.Clear();

            if (e.OldValue != null)
            {
                INotifyCollectionChanged notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    //
                    // Unhook events from previous collection.
                    //
                    notifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(c.ConnectionsSource_CollectionChanged);
                }
            }

            if (e.NewValue != null)
            {
                IEnumerable enumerable = e.NewValue as IEnumerable;
                if (enumerable != null)
                {
                    //
                    // Populate 'Connections' from 'ConnectionsSource'.
                    //
                    foreach (object obj in enumerable)
                    {
                        c.Connections.Add(obj);
                    }
                }

                INotifyCollectionChanged notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    //
                    // Hook events in new collection.
                    //
                    notifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(c.ConnectionsSource_CollectionChanged);
                }
            }
        }

        /// <summary>
        /// Event raised when a connection has been added to or removed from the collection assigned to 'ConnectionsSource'.
        /// </summary>
        private void ConnectionsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                //
                // 'ConnectionsSource' has been cleared, also clear 'Connections'.
                //
                Connections.Clear();
            }
            else
            {
                if (e.OldItems != null)
                {
                    //
                    // For each item that has been removed from 'ConnectionsSource' also remove it from 'Connections'.
                    //
                    foreach (object obj in e.OldItems)
                    {
                        Connections.Remove(obj);
                    }
                }

                if (e.NewItems != null)
                {
                    //
                    // For each item that has been added to 'ConnectionsSource' also add it to 'Connections'.
                    //
                    foreach (object obj in e.NewItems)
                    {
                        Connections.Add(obj);
                    }
                }
            }
        }

        /// <summary>
        /// Event raised when a new collection has been assigned to the 'TemperaryNodesSource' property.
        /// </summary>
        private static void TemperaryNodesSource_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DesignerView c = (DesignerView)d;

            //
            // Clear 'TemperaryNodes'.
            //
            c.TemperaryNodes.Clear();

            if (e.OldValue != null)
            {
                INotifyCollectionChanged notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    //
                    // Unhook events from previous collection.
                    //
                    notifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(c.TemperaryNodesSource_CollectionChanged);
                }
            }

            if (e.NewValue != null)
            {
                IEnumerable enumerable = e.NewValue as IEnumerable;
                if (enumerable != null)
                {
                    //
                    // Populate 'TemperaryNodes' from 'TemperaryNodesSource'.
                    //
                    foreach (object obj in enumerable)
                    {
                        c.TemperaryNodes.Add(obj);
                    }
                }

                INotifyCollectionChanged notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                {
                    //
                    // Hook events in new collection.
                    //
                    notifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(c.TemperaryNodesSource_CollectionChanged);
                }
            }
        }

        /// <summary>
        /// Event raised when a connection has been added to or removed from the collection assigned to 'TemperaryNodesSource'.
        /// </summary>
        private void TemperaryNodesSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                //
                // 'TemperaryNodesSource' has been cleared, also clear 'TemperaryNodes'.
                //
                TemperaryNodes.Clear();
            }
            else
            {
                if (e.OldItems != null)
                {
                    //
                    // For each item that has been removed from 'TemperaryNodesSource' also remove it from 'TemperaryNodes'.
                    //
                    foreach (object obj in e.OldItems)
                    {
                        TemperaryNodes.Remove(obj);
                    }
                }

                if (e.NewItems != null)
                {
                    //
                    // For each item that has been added to 'TemperaryNodesSource' also add it to 'TemperaryNodes'.
                    //
                    foreach (object obj in e.NewItems)
                    {
                        TemperaryNodes.Add(obj);
                    }
                }
            }
        }

        /// <summary>
        /// Called after the visual tree of the control has been built.
        /// Search for and cache references to named parts defined in the XAML control template for DesignerView.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //
            // Cache the parts of the visual tree that we need access to later.
            //

            this.nodeItemsControl = (NodeItemsControl)this.Template.FindName("PART_NodeItemsControl", this);
            if (this.nodeItemsControl == null)
            {
                throw new ApplicationException("Failed to find 'PART_NodeItemsControl' in the visual tree for 'DesignerView'.");
            }

            //
            // Synchronize initial selected nodes to the NodeItemsControl.
            //
            if (this.initialSelectedNodes != null && this.initialSelectedNodes.Count > 0)
            {
                foreach (var node in this.initialSelectedNodes)
                {
                    this.nodeItemsControl.SelectedItems.Add(node);
                }
            }

            this.initialSelectedNodes = null; // Don't need this any more.

            this.nodeItemsControl.SelectionChanged += new SelectionChangedEventHandler(nodeItemsControl_SelectionChanged);

            this.connectionItemsControl = (ItemsControl)this.Template.FindName("PART_ConnectionItemsControl", this);
            if (this.connectionItemsControl == null)
            {
                throw new ApplicationException("Failed to find 'PART_ConnectionItemsControl' in the visual tree for 'DesignerView'.");
            }

            this.dragSelectionCanvas = (FrameworkElement)this.Template.FindName("PART_DragSelectionCanvas", this);
            if (this.dragSelectionCanvas == null)
            {
                throw new ApplicationException("Failed to find 'PART_DragSelectionCanvas' in the visual tree for 'DesignerView'.");
            }

            this.dragSelectionBorder = (FrameworkElement)this.Template.FindName("PART_DragSelectionBorder", this);
            if (this.dragSelectionBorder == null)
            {
                throw new ApplicationException("Failed to find 'PART_dragSelectionBorder' in the visual tree for 'DesignerView'.");
            }
        }

        /// <summary>
        /// Event raised when the selection in 'nodeItemsControl' changes.
        /// </summary>
        private void nodeItemsControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectionChanged != null)
            {
                this.SelectionChanged(this, new SelectionChangedEventArgs(ListBox.SelectionChangedEvent, e.RemovedItems, e.AddedItems));
            }
        }

        /// <summary>
        /// Find the max ZIndex of all the nodes.
        /// </summary>
        internal int FindMaxZIndex()
        {
            if (this.nodeItemsControl == null)
            {
                return 0;
            }

            int maxZ = 0;

            for (int nodeIndex = 0; ; ++nodeIndex)
            {
                NodeItem nodeItem = (NodeItem)this.nodeItemsControl.ItemContainerGenerator.ContainerFromIndex(nodeIndex);
                if (nodeItem == null)
                {
                    break;
                }

                if (nodeItem.ZIndex > maxZ)
                {
                    maxZ = nodeItem.ZIndex;
                }
            }

            return maxZ;
        }

        /// <summary>
        /// Find the NodeItem UI element that is associated with 'node'.
        /// 'node' can be a view-model object, in which case the visual-tree
        /// is searched for the associated NodeItem.
        /// Otherwise 'node' can actually be a 'NodeItem' in which case it is 
        /// simply returned.
        /// </summary>
        internal NodeItem FindAssociatedNodeItem(object node)
        {
            NodeItem nodeItem = node as NodeItem;
            if (nodeItem == null)
            {
                nodeItem = nodeItemsControl.FindAssociatedNodeItem(node);
            }
            return nodeItem;
        }

        #endregion Private Methods
    }
}
