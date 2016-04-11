using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.Enums;
using VisualProgrammer.Utilities;

namespace VisualProgrammer.ViewModels.CompilerStatus
{
    public class LogViewModel : AbstractModelBase
    {
        #region Private Data Members

        /// <summary>
        /// The type of log message
        /// </summary>
        private WriteType type;

        /// <summary>
        /// The log message
        /// </summary>
        private string message;

        #endregion Private Data Members

        public LogViewModel(WriteType type, string message)
        {
            this.type = type;
            this.message = message;
        }

        /// <summary>
        /// Holds the type of message that is
        /// logged
        /// </summary>
        public WriteType Type
        {
            get
            {
                return type;
            }
            set
            {
                if (type == value)
                    return;

                type = value;

                OnPropertyChanged("Type");
            }
        }

        /// <summary>
        /// Hold the message that is logged
        /// </summary>
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                if (message == value)
                    return;

                message = value;

                OnPropertyChanged("Message");
            }
        }
    }
}
