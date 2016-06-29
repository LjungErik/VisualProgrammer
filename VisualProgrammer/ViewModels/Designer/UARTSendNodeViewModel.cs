using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.Data;
using VisualProgrammer.Data.Actions;

namespace VisualProgrammer.ViewModels.Designer
{
    public class UARTSendNodeViewModel : NodeViewModel
    {
        #region Private Data Members

        private UARTSendAction action = null;

        #endregion Private Data Members

        public UARTSendNodeViewModel()
        {
            action = new UARTSendAction();
            action.Message = "Hello World!";

            model = new Node();
            model.Action = action;
            model.NodeGuid = Guid.NewGuid().ToString();

            InputConnector = new ConnectorViewModel();
            OutputConnector = new ConnectorViewModel();
        }

        public UARTSendNodeViewModel(Node node)
        {
            model = node;
            action = (UARTSendAction)node.Action;

            InputConnector = new ConnectorViewModel();
            OutputConnector = new ConnectorViewModel();
        }

        /// <summary>
        /// The message to be sent over UART
        /// </summary>
        public string Message
        {
            get { return action.Message; }
            set
            {
                if (action.Message == value)
                    return;

                action.Message = value;

                OnPropertyChanged("Message");
            }
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
