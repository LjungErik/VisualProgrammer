using VisualProgrammer.Actions;
using VisualProgrammer.ViewModels.Designer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrammer.Utilities
{
    public class ModelCollector
    {
        public static List<IRobotAction> GetModels(StartNodeViewModel start)
        {
            List<IRobotAction> models = new List<IRobotAction>();

            NodeViewModel node = start;

            while((node = GetNextNode(node)) != null)
            {
                if (node.Model != null)
                    models.Add(node.Model);
            }

            return models;
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
