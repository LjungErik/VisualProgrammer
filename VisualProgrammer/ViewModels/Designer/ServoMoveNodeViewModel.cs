
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VisualProgrammer.Data;
using VisualProgrammer.Data.Actions;

namespace VisualProgrammer.ViewModels.Designer
{
    public class ServoMoveNodeViewModel : NodeViewModel
    {

        private ServoMoveAction action = null;

        public ServoMoveNodeViewModel()
        {
            action = new ServoMoveAction();
            action.Degrees = 180;
            action.Servo = 1;

            model = new Node();
            model.Action = action;
            model.NodeGuid = Guid.NewGuid().ToString();
            //Set up connectors
            InputConnector = new ConnectorViewModel();
            OutputConnector = new ConnectorViewModel();
        }

        /// <summary>
        /// The Servo that is to be moved
        /// </summary>
        public int Servo
        {
            get { return action.Servo; }
            set
            {
                if (action.Servo == value)
                    return;

                action.Servo = value;

                OnPropertyChanged("Servo");
            }
        }

        /// <summary>
        /// The number of degrees to move the servo
        /// </summary>
        public int Degrees
        {
            get { return action.Degrees; }
            set 
            {
                if (action.Degrees == value)
                    return;

                action.Degrees = value;

                OnPropertyChanged("Degrees");
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
