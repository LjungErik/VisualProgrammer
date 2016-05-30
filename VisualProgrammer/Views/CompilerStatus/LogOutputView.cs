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

        public static readonly DependencyProperty LogsSourceProperty =
            DependencyProperty.Register("LogsSource", typeof(IEnumerable), typeof(LogOutputView));

        #endregion Dependency Property/Event Definitions

        #region Private Data Members

        private ScrollViewer scrollViewer = null;

        #endregion Private Data Members

        public LogOutputView()
        {
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

        static LogOutputView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LogOutputView), new FrameworkPropertyMetadata(typeof(LogOutputView)));
        }

        #endregion Private Methods

    }
}
