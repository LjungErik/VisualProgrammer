using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.Utilities;
using VisualProgrammer.ViewModels.CompilerStatus;

namespace VisualProgrammer.ViewModels
{
    public class CompilerStatusViewModel : AbstractModelBase
    {

        #region Private Data Members

        /// <summary>
        /// The representation of the output from the logger
        /// </summary>
        private LogOutputViewModel logOutput = null;

        /// <summary>
        /// The representation of the progress for the logger
        /// </summary>
        private LogProgressViewModel logProgress = null;

        #endregion Private Data Members

        public CompilerStatusViewModel()
        {
            logOutput = new LogOutputViewModel();
            logProgress = new LogProgressViewModel();
        }

        /// <summary>
        /// Holds the active output produced by the current logger
        /// </summary>
        public LogOutputViewModel LogOutput
        {
            get
            {
                return logOutput;
            }
            set
            {
                if (logOutput == value)
                    return;

                logOutput = value;

                OnPropertyChanged("LogOutput");
            }
        }

        /// <summary>
        /// Holds the progress and status of the current logger
        /// </summary>
        public LogProgressViewModel LogProgress
        {
            get
            {
                return logProgress;
            }
            set
            {
                if (logProgress == value)
                    return;

                logProgress = value;

                OnPropertyChanged("LogProgress");
            }
        }
    }
}
