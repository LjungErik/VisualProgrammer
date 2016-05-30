using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.Enums;
using VisualProgrammer.Utilities;
using VisualProgrammer.Utilities.Logger;

namespace VisualProgrammer.ViewModels.CompilerStatus
{
    public class LogOutputViewModel : AbstractModelBase
    {
        #region Private Data Members

        /// <summary>
        /// Holds the activlly logged events
        /// </summary>
        private ObservableCollection<LogViewModel> logs = null;

        private CompileLogger logger = null;

        #endregion Private Data Members

        public LogOutputViewModel()
        {
            logs = new ObservableCollection<LogViewModel>();
        }

        public ObservableCollection<LogViewModel> Logs
        {
            get
            {
                return logs;
            }
        }

        public CompileLogger Logger
        {
            get
            {
                return logger;
            }
            set
            {
                if (logger == value)
                    return;

                if(logger != null)
                {
                   
                    
                    logger.LogUpdated -= new CompilerLogChangedEventHandler(CompilerLog_LogUpdated);
                }

                //Clear the log of all old logs
                logs.Clear();
                logger = value;

                if(logger != null)
                {
                    logger.LogUpdated += new CompilerLogChangedEventHandler(CompilerLog_LogUpdated);
                }
            }
        }

        #region Private Methods

        private void CompilerLog_LogUpdated(object sender, CompilerLogChangedEventArgs e)
        {
            WriteType type = e.Type;
            string message = e.LogMessage;

            Logs.Add(new LogViewModel(type, message));
        }

        #endregion Private Methods
    }
}
