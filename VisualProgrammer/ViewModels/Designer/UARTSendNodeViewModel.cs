using VisualProgrammer.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrammer.ViewModels.Designer
{
    public class UARTSendNodeViewModel : NodeViewModel
    {
        #region Private Data Members

        private UARTSendAction model = null;

        #endregion Private Data Members

        public UARTSendNodeViewModel()
        {
            model = new UARTSendAction("Hello World");

            InputConnector = new ConnectorViewModel();
            OutputConnector = new ConnectorViewModel();

        }

        /// <summary>
        /// The message to be sent over UART
        /// </summary>
        public string Message
        {
            get { return model.Message; }
            set
            {
                if (model.Message == value)
                    return;

                model.Message = value;

                OnPropertyChanged("Message");
            }
        }

        /// <summary>
        /// The Model that represents the action of the node
        /// </summary>
        public override IRobotAction Model
        {
            get { return model; }
        }

        public override double Width
        {
            get
            {
                return 160;
            }
        }

        public override double Height
        {
            get
            {
                return 154;
            }
        }
    }
}
