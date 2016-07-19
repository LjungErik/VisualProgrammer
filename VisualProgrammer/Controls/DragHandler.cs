using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using VisualProgrammer.Utilities;

namespace VisualProgrammer.Controls
{
    public class DragHandler
    {
        #region Private Members

        private static FrameworkElement root = null;

        private static IDraggable draggedItem = null;

        private static IDropView dropView = null;

        #endregion Private Members

        public static void SetRoot(FrameworkElement element)
        {
            root = element;
        }

        public static IDraggable DraggedItem
        {
            get
            {
                return draggedItem;
            }
            set
            {
                if(draggedItem != null)
                {
                    draggedItem.OnDragging -= new DraggableDragEventHandler(Dragging);
                    draggedItem.OnDragCompleted -= new DraggableDropEventHandler(DragCompleted);
                }

                draggedItem = value;

                if(draggedItem != null)
                {
                    draggedItem.OnDragging += new DraggableDragEventHandler(Dragging);
                    draggedItem.OnDragCompleted += new DraggableDropEventHandler(DragCompleted);
                }
            }
        }

        private static void Dragging(object sender, DraggableDragEventArgs e)
        {
            if (DraggedItem == null)
                return;

            dropView = GetDropViewAtMouse();

            if (dropView != null)
                dropView.DragOver(DraggedItem);
            else
                ShowMouseError();
        }

        private static void DragCompleted(object sender, DraggableDropEventArgs e)
        {
            if (DraggedItem == null)
                return;

            dropView = GetDropViewAtMouse();

            if (dropView != null)
                dropView.Dropped(DraggedItem);

            DraggedItem = null;
            Mouse.OverrideCursor = null;
        }

        private static IDropView GetDropViewAtMouse()
        {
            if(root != null)
            {
                Point point = Mouse.GetPosition(root);

                var result = VisualTreeHelper.HitTest(root, point);
                var element = result != null ? result.VisualHit as FrameworkElement : null;

                if(element != null)
                    return WpfUtils.FindVisualParentWithType<IDropView>(element);
            }
            return null;
        }

        private static void ShowMouseError() 
        {
            Mouse.OverrideCursor = Cursors.No;
        }
    }
}
