using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VisualProgrammer.Views.Restructure.Designer;

namespace VisualProgrammer.Factory.MouseActions
{
    public class SelectMouseAction : IMouseAction
    {
        #region Private Data Members

        private const int DRAG_THRESHOLD = 3;

        private bool isLeftMouseDown = false;

        private bool isDragging = false;

        private Point originalStartPosition;

        #endregion Private Data Members

        public Cursor MouseCursor
        {
            get { return Cursors.Hand; }
        }

        public void OnMouseDown(DesignView designView, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                originalStartPosition = e.GetPosition(designView);
                isLeftMouseDown = true;

                designView.CaptureMouse();

                e.Handled = true;
            }
        }

        public void OnMouseMove(DesignView designView, MouseEventArgs e)
        {
            if(isDragging)
            {
                Point currentPosition = e.GetPosition(designView);
                designView.UpdateSelectionArea(currentPosition, originalStartPosition);

                e.Handled = true;
            }
            else if(isLeftMouseDown)
            {
                Point currentPosition = e.GetPosition(designView);
                var offset = currentPosition - originalStartPosition;
                double deltaLength = offset.Length;
                if(deltaLength > DRAG_THRESHOLD)
                {
                    isDragging = true;
                    designView.InitSelectionArea(currentPosition, originalStartPosition);
                }
                e.Handled = true;
            }
        }

        public void OnMouseUp(DesignView designView, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Released && isLeftMouseDown)
            {
                if(isDragging)
                {
                    isDragging = false;
                    designView.ApplySelection();
                }
                isLeftMouseDown = false;
                designView.ReleaseMouseCapture();
            }
        }
    }
}
