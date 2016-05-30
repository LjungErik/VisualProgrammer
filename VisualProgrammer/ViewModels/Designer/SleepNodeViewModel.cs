using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.Actions;

namespace VisualProgrammer.ViewModels.Designer
{
    public class SleepNodeViewModel : NodeViewModel
    {
        #region Private Data Members

        private SleepAction model = null;

        #endregion Private Data Members

        public SleepNodeViewModel()
        {
            model = new SleepAction(1000);

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
                return model.Time;
            }
            set
            {
                if (model.Time == value)
                    return;

                model.Time = value;

                OnPropertyChanged("Time");
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
