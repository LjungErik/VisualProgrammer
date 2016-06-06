using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VisualProgrammer.ViewModels.Designer;
using VisualProgrammer.Views.Designer.Events;

namespace VisualProgrammer.Views.Designer
{
    public partial class DesignView
    {
        #region Private Data Members

        private const int SCROLL_AREA = 10;

        private List<object> nodes = null;

        #endregion Private Data Members

        private void Node_DragStarted(object sender, NodeDragStartedEventArgs e)
        {
            e.Handled = true;

            nodes = new List<object>();

            foreach(NodeViewModel node in nodeControl.SelectedItems)
            {
                node.SaveLocation();
            }

            var eventArgs = new NodeDragStartedEventArgs(NodeDragStartedEvent, this, nodeControl.SelectedItems);
            RaiseEvent(eventArgs);

            e.Cancel = eventArgs.Cancel;
        }

        private void Node_Dragging(object sender, NodeDraggingEventArgs e)
        {
            e.Handled = true;

            foreach(NodeViewModel node in nodeControl.SelectedItems)
            {
                node.X += e.HorizontalChange;
                node.Y += e.VerticalChange;
            }

            RaiseEvent(new NodeDraggingEventArgs(NodeDraggingEvent, this, nodeControl.SelectedItems, e.HorizontalChange, e.VerticalChange));
        }

        private void Node_DragCompleted(object sender, NodeDragCompletedEventArgs e)
        {
            e.Handled = true;

            var eventArgs = new NodeDragCompletedEventArgs(NodeDragCompletedEvent, this, nodeControl.SelectedItems);
            RaiseEvent(eventArgs);

            if(eventArgs.Cancel)
            {
                foreach(NodeViewModel node in nodeControl.SelectedItems)
                    node.ResetLocation();
            }
        }
    }
}
