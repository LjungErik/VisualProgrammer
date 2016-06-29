using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.Data;
using VisualProgrammer.Data.Actions;

namespace VisualProgrammer.ViewModels.Designer
{
    public class SleepNodeViewModel : NodeViewModel
    {
        #region Private Data Members

        private SleepAction action = null;

        #endregion Private Data Members

        public SleepNodeViewModel()
        {
            action = new SleepAction();
            action.Time = 1000;

            model = new Node();
            model.Action = action;
            model.NodeGuid = Guid.NewGuid().ToString();
            //Set up connectors
            InputConnector = new ConnectorViewModel();
            OutputConnector = new ConnectorViewModel();
        }

        public SleepNodeViewModel(Node node)
        {
            model = node;
            action = (SleepAction)node.Action;

            //Set up connectors
            InputConnector = new ConnectorViewModel();
            OutputConnector = new ConnectorViewModel();
        }

        /// <summary>
        /// Holds the time to sleep (pause execution)
        /// </summary>
        public int Time
        {
            get
            {
                return action.Time;
            }
            set
            {
                if (action.Time == value)
                    return;

                action.Time = value;

                OnPropertyChanged("Time");
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
