using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VisualProgrammer.Utilities;

namespace VisualProgrammer.Views.CompilerStatus
{
    /// <summary>
    /// Represents the log window for the 'CompilerStatusWindow'
    /// </summary>
    public class LogOutputView : Control
    {
        #region Dependency Property/Event Definitions

        private static readonly DependencyPropertyKey LogsPropertyKey =
            DependencyProperty.RegisterReadOnly("Logs", typeof(ImpObservableCollection<object>), typeof(LogOutputView),
                new FrameworkPropertyMetadata());
        public static readonly DependencyProperty LogsProperty = LogsPropertyKey.DependencyProperty;

        public static readonly DependencyProperty LogsSourceProperty =
            DependencyProperty.Register("LogsSource", typeof(IEnumerable), typeof(LogOutputView),
                new FrameworkPropertyMetadata(LogsSource_PropertyChanged));

        #endregion Dependency Property/Event Definitions

        #region Private Data Members

        private ScrollViewer scrollViewer = null;

        #endregion Private Data Members

        public LogOutputView()
        {
            this.Logs = new ImpObservableCollection<object>();
        }

        /// <summary>
        /// Collection of logs made to the compilerstatusview
        /// </summary>
        public ImpObservableCollection<object> Logs
        {
            get
            {
                return (ImpObservableCollection<object>)GetValue(LogsProperty);
            }
            set
            {
                SetValue(LogsPropertyKey, value);
            }
        }

        /// <summary>
        /// A reference to the collection that is the source 
        /// used to populate 'Logs'.
        /// </summary>
        public IEnumerable LogsSource
        {
            get
            {
                return (IEnumerable)GetValue(LogsSourceProperty);
            }
            set
            {
                SetValue(LogsSourceProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.scrollViewer = (ScrollViewer)this.Template.FindName("PART_ScrollViewer", this);
            if(this.scrollViewer == null)
            {
                throw new ApplicationException("Failed to find 'PART_ScrollViewer' in the virtual tree for 'LogOutputView'.");
            }
        }

        #region Private Methods

        /// <summary>
        /// Static constructor
        /// </summary>
        static LogOutputView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LogOutputView), new FrameworkPropertyMetadata(typeof(LogOutputView)));
        }

        /// <summary>
        /// Event raised when the LogsSource property is changed
        /// </summary>
        private static void LogsSource_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LogOutputView c = (LogOutputView)d;

            c.Logs.Clear();

            if(e.OldValue != null)
            {
                var notifyCollectionChanged = e.OldValue as INotifyCollectionChanged;
                if(notifyCollectionChanged != null)
                {
                    //Unhook event from the previous collection
                    notifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(c.LogsSource_CollectionChanged);
                }
            }

            if(e.NewValue != null)
            {
                var enumrable = e.NewValue as IEnumerable;
                if(enumrable != null)
                {
                    foreach(object obj in enumrable)
                    {
                        c.Logs.Add(obj);
                    }
                }

                var notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
                if(notifyCollectionChanged != null)
                {
                    //Hook event to new collection
                    notifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(c.LogsSource_CollectionChanged);
                }
            }
        }

        /// <summary>
        /// Event raised when logs are added or removed from the collection assigned to 'LogsSource'
        /// </summary>
        private void LogsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Reset)
            {
                Logs.Clear();
            }
            else
            {
                if(e.OldItems != null)
                {
                    //Remove all newly removed items from 'Logs'
                    foreach(object obj in e.OldItems)
                    {
                        Logs.Remove(obj);
                    }
                    //Scroll to the bottom (follow the log stream)
                    scrollViewer.ScrollToBottom();
                }

                if(e.NewItems != null)
                {
                    //Add all newly added items to 'Logs'
                    foreach(object obj in e.NewItems)
                    {
                        Logs.Add(obj);
                    }
                    //Scroll to the bottom (follow the log stream)
                    scrollViewer.ScrollToBottom();
                }
            }
        }

        #endregion Private Methods

    }
}
