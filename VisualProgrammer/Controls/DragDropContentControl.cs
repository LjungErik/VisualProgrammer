using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace VisualProgrammer.Controls
{
    public class DragDropContentControl : ContentControl, IDraggable
    {
        #region Private Data Members

        private bool isDragging = false;

        #endregion Private Data Members

        protected void BeginDragAndDrop()
        {
            isDragging = true;
            this.CaptureMouse();

            DragHandler.DraggedItem = this;
        }

        private void EndDragAndDrop()
        {
            isDragging = false;
            this.ReleaseMouseCapture();

            if (OnDragCompleted != null)
                OnDragCompleted(this, new DraggableDropEventArgs());
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            if(e.ChangedButton == MouseButton.Left)
                BeginDragAndDrop();
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if(isDragging)
            {
                if (OnDragging != null)
                    OnDragging(this, new DraggableDragEventArgs());
            }
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);

            if(isDragging)
            {
                EndDragAndDrop();
            }
        }
    
        public event DraggableDragEventHandler OnDragging;

        public event DraggableDropEventHandler OnDragCompleted;

        public new void Drop()
        {
            EndDragAndDrop();
        }
    }
}
