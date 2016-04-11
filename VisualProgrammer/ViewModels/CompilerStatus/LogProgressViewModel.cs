using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.Enums;
using VisualProgrammer.Utilities;
using VisualProgrammer.Utilities.Logger;

namespace VisualProgrammer.ViewModels.CompilerStatus
{
    public class LogProgressViewModel : AbstractModelBase
    {
        #region Private Data Member

        /// <summary>
        /// The progress of the logger
        /// </summary>
        private double progress = 0.0;

        private StatusType status;

        /// <summary>
        /// The logger to listen for progress events on
        /// </summary>
        private CompileLogger logger = null;

        #endregion Private Data Member

        public LogProgressViewModel()
        {}

        /// <summary>
        /// Holds the progress of the compiler
        /// </summary>
        public double Progress
        {
            get 
            { 
                return progress; 
            }
            set
            {
                if (progress == value)
                    return;

                progress = value;

                OnPropertyChanged("Progress");
            }
        }

        /// <summary>
        /// Holds the status of the compiler
        /// e.g. if it is Processing or Stopped
        /// </summary>
        public StatusType Status
        {
            get
            {
                return status;
            }
            set
            {
                if (status == value)
                    return;

                status = value;

                OnPropertyChanged("Status");
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

                if (logger != null)
                {
                    logger.ProgressChanged -= new CompilerLogProgressChangedEventHandler(CompileLogger_ProgressChanged);
                    logger.StatusChanged -= new CompilerLogStatusChangedEventHandler(CompileLogger_StatusChanged);
                }

                logger = value;

                if(logger != null)
                {
                    logger.ProgressChanged += new CompilerLogProgressChangedEventHandler(CompileLogger_ProgressChanged);
                    logger.StatusChanged += new CompilerLogStatusChangedEventHandler(CompileLogger_StatusChanged);
                }
            }
        }

        #region Private Methods

        private void CompileLogger_ProgressChanged(object sender, CompilerLogProgressChangedEventArgs e)
        {
            Progress = e.Progress;
        }

        private void CompileLogger_StatusChanged(object sender, CompilerLogStatusChangedEventArgs e)
        {
            Status = e.Status;
        }

        #endregion Private Methods
    }
}
