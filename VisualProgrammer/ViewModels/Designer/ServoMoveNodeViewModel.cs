
using VisualProgrammer.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrammer.ViewModels.Designer
{
    public class ServoMoveNodeViewModel : NodeViewModel
    {
        #region Private Data Members

        private ServoMoveAction model = null;

        #endregion Private Data Members

        public ServoMoveNodeViewModel()
        {
            model = new ServoMoveAction(1, 180);
            //Set up connectors
            InputConnector = new ConnectorViewModel();
            OutputConnector = new ConnectorViewModel();
        }

        /// <summary>
        /// The Servo that is to be moved
        /// </summary>
        public int Servo
        {
            get { return model.Servo; }
            set
            {
                if (model.Servo == value)
                    return;

                model.Servo = value;

                OnPropertyChanged("Servo");
            }
        }

        /// <summary>
        /// The number of degrees to move the servo
        /// </summary>
        public int Degrees
        {
            get { return model.Degrees; }
            set 
            {
                if (model.Degrees == value)
                    return;

                model.Degrees = value;

                OnPropertyChanged("Degrees");
            }
        }

        /// <summary>
        /// The Model that represents the action of the node
        /// Default is to send null
        /// </summary>
        public override IRobotAction Model
        {
            get { return model; }
        }
    }
}
