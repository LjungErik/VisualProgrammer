using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace VisualProgrammer.Views.Designer
{
    public class NodeControl : ListBox
    {
        public Node FindAssociatedNode(object nodeDataContext)
        {
            return (Node)this.ItemContainerGenerator.ContainerFromItem(nodeDataContext);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new Node();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is Node;
        }
    }
}
