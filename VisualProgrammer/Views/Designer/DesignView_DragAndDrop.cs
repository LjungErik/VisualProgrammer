using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VisualProgrammer.Controls;
using VisualProgrammer.ViewModels.Designer;
using VisualProgrammer.Views.Designer.Events;
using VisualProgrammer.Views.Toolbox;

namespace VisualProgrammer.Views.Designer
{
    public partial class DesignView : IDropView
    {
        public new void DragOver(IDraggable dragged)
        {
            RaiseEvent(new DragDropEventArgs(DraggedOverEvent, this, dragged));

            dragged.Drop();
        }

        public void Dropped(IDraggable dragged)
        {
            var eventArgs = new DragDropEventArgs(DroppedOverEvent, this, dragged);

            RaiseEvent(eventArgs);

            if(eventArgs.ReturnItem != null)
            {
                Node node = FindNodeContainer(eventArgs.ReturnItem);

                if(node != null)
                {
                    this.ClearSelected();
                    node.BeginDrag(Mouse.GetPosition(this));
                }
            }
        }

        private Node FindNodeContainer(object nodeContext)
        {
            var nodeVM = nodeContext as NodeViewModel;

            if (nodeVM != null)
                return nodeControl.FindAssociatedNode(nodeVM);

            return null;
        }
    }
}
