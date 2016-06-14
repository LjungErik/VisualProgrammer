using VisualProgrammer.ViewModels.Designer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.Data.Actions;

namespace VisualProgrammer.Utilities
{
    public class ModelCollector
    {
        public static List<RobotAction> GetModels(StartNodeViewModel start)
        {
            List<RobotAction> actions = new List<RobotAction>();

            NodeViewModel node = start;

            while((node = GetNextNode(node)) != null)
            {
                if (node.Action != null)
                    actions.Add(node.Action);
            }

            return actions;
        }

        #region Private Methods

        public static NodeViewModel GetNextNode(NodeViewModel node)
        {
            if(node != null)
            {
                ConnectorViewModel outputConnector = node.OutputConnector;
                if(outputConnector != null && outputConnector.AttachedConnection != null)
                {
                    ConnectionViewModel connection = outputConnector.AttachedConnection;
                    if(connection != null && connection.DestConnector != null)
                    {
                        ConnectorViewModel destConnector = connection.DestConnector;
                        return destConnector.ParentNode;
                    }
                }
            }
            return null;
        }

        #endregion Private Methods
    }
}
