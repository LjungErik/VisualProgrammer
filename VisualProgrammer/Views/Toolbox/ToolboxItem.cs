using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VisualProgrammer.Controls;

namespace VisualProgrammer.Views.Toolbox
{
    public class ToolboxItem : ContentControl, IDraggable
    {
        #region Private Data Members

        private bool isDragging = false;

        #endregion Private Data Members

        #region Dependency Property/Event Definitions

        public static readonly RoutedEvent ToolboxItemDropCanceledEvent =
            EventManager.RegisterRoutedEvent("ToolboxItemDropCanceled", RoutingStrategy.Bubble, typeof(ToolboxItemDropCanceledEventHandler), typeof(ToolboxItem));


        #endregion Dependency Property/Event Definitions

        static ToolboxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolboxItem), new FrameworkPropertyMetadata(typeof(ToolboxItem)));
        }

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

            var eventArgs = new DraggableDropEventArgs();

            if (OnDragCompleted != null)
                OnDragCompleted(this, eventArgs);

            if (eventArgs.Failed)
                RaiseEvent(new ToolboxItemEventArgs(ToolboxItemDropCanceledEvent, this, this));
        }

        #region Mouse Methods

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            if (e.ChangedButton == MouseButton.Left)
                BeginDragAndDrop();
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (isDragging)
            {
                if (OnDragging != null)
                    OnDragging(this, new DraggableDragEventArgs());
            }
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);

            if (isDragging)
            {
                EndDragAndDrop();
            }
        }

        #endregion Mouse Methods

        public event DraggableDragEventHandler OnDragging;

        public event DraggableDropEventHandler OnDragCompleted;

        public new void Drop()
        {
            EndDragAndDrop();
        }
    }
}
