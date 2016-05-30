using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.Enums;

namespace VisualProgrammer.Utilities.Logger
{
    #region Events Args/Delegates

    public class CompilerLogChangedEventArgs : EventArgs
    {
        public CompilerLogChangedEventArgs(WriteType type, string logMessage)
        {
            this.Type = type;
            this.LogMessage = logMessage;
        }

        public WriteType Type { get; private set; }
        public string LogMessage { get; private set; }

    }

    public delegate void CompilerLogChangedEventHandler(object sender, CompilerLogChangedEventArgs e);

    public class CompilerLogProgressChangedEventArgs : EventArgs
    {
        public CompilerLogProgressChangedEventArgs(double progress)
        {
            this.Progress = progress;
        }

        public double Progress { get; private set; }
    }

    public delegate void CompilerLogProgressChangedEventHandler(object sender, CompilerLogProgressChangedEventArgs e);

    public class CompilerLogStatusChangedEventArgs : EventArgs
    {
        public CompilerLogStatusChangedEventArgs(StatusType status)
        {
            Status = status;
        }

        public StatusType Status { get; private set; }
    }

    public delegate void CompilerLogStatusChangedEventHandler(object sender, CompilerLogStatusChangedEventArgs e);

    #endregion Events Args/Delegates

    public class CompileLogger
    {
        #region Event

        public event CompilerLogChangedEventHandler LogUpdated;

        public event CompilerLogProgressChangedEventHandler ProgressChanged;

        public event CompilerLogStatusChangedEventHandler StatusChanged;

        #endregion Event

        #region Writes

        public void WriteInfo(string info)
        {
            string message = "[INFO] - " + info;

            CompilerLogChangedEventHandler handler = LogUpdated;
            if(handler != null)
            {
                handler(this, new CompilerLogChangedEventArgs(WriteType.Info, message));
            }
        }

        public void WriteError(string error)
        {
            string message = "[ERROR] - " + error;

            CompilerLogChangedEventHandler handler = LogUpdated;
            if(handler != null)
            {
                handler(this, new CompilerLogChangedEventArgs(WriteType.Error, message));
            }
        }

        public void WriteWarning(string warning)
        {
            string message = "[WARNING] - " + warning;

            CompilerLogChangedEventHandler handler = LogUpdated;
            if(handler != null)
            {
                handler(this, new CompilerLogChangedEventArgs(WriteType.Warning, message));
            }
        }

        #endregion Writes

        #region Progress

        public void SetProgress(double value)
        {
            CompilerLogProgressChangedEventHandler handler = (CompilerLogProgressChangedEventHandler)ProgressChanged;
            if(handler != null)
            {
                handler(this, new CompilerLogProgressChangedEventArgs(value));
            }
        }

        #endregion Progress

        #region Status

        public void SetStatus(StatusType status)
        {
            CompilerLogStatusChangedEventHandler handler = (CompilerLogStatusChangedEventHandler)StatusChanged;
            if(handler != null)
            {
                handler(this, new CompilerLogStatusChangedEventArgs(status));
            }
        }

        #endregion Status
    }
}
