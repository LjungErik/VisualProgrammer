using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace VisualProgrammer.ViewModels.Designer
{
    public class StartNodeViewModel : NodeViewModel
    {
        public StartNodeViewModel()
        {
            OutputConnector = new ConnectorViewModel();
        }

        /// <summary>
        /// Overrides the InputConnector because not needed on start node
        /// </summary>
        public override ConnectorViewModel InputConnector
        {
            get { return null; }
        }

        /// <summary>
        /// Override to only return OutputConnector as AttachedConnection
        /// </summary>
        public override ICollection<ConnectionViewModel> AttachedConnections
        {
            get
            {
                List<ConnectionViewModel> attachedConnections = new List<ConnectionViewModel>();

                attachedConnections.Add(OutputConnector.AttachedConnection);

                return attachedConnections;
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
