using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VisualProgrammer.ViewModels.Designer;
using VisualProgrammer.Views.Designer;

namespace VisualProgrammer.Factory.MouseActions
{
    public class MoveMouseAction : IMouseAction
    {
        #region Private Data Members

        private const int DRAG_THRESHOLD = 3;

        private bool isLeftMouseDown = false;

        private bool isDragging = false;

        private Point lastPosition;

        #endregion Private Data Members

        public Cursor MouseCursor
        {
            get { return Cursors.SizeAll; }
        }

        public void OnMouseDown(DesignView designView, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                isLeftMouseDown = true;
                lastPosition = e.GetPosition(designView);

                designView.CaptureMouse();

                e.Handled = true;
            }
        }

        public void OnMouseMove(DesignView designView, MouseEventArgs e)
        {
            if(isDragging)
            {
                Point currentPosition = e.GetPosition(designView);
                var offset = currentPosition - lastPosition;
                if(offset.X != 0 || offset.Y != 0)
                {
                    MoveDesignView(designView, offset.X, offset.Y);

                    lastPosition = currentPosition;
                }
                e.Handled = true;
            }
            else if(isLeftMouseDown)
            {
                Point currentPosition = e.GetPosition(designView);

                var offset = currentPosition - lastPosition;
                double deltaLength = offset.Length;
                if(deltaLength > DRAG_THRESHOLD)
                {
                    isDragging = true;
                    lastPosition = currentPosition;
                }
                e.Handled = true;
            }
        }

        public void OnMouseUp(DesignView designView, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                if (isDragging)
                {
                    isDragging = false;
                }
                designView.ReleaseMouseCapture();
                isLeftMouseDown = false;
                e.Handled = true;
            }
        }

        private void MoveDesignView(DesignView designView, double horizontalChange, double verticalChange)
        {
            foreach(NodeViewModel node in designView.NodesSource)
            {
                /* Shift the position of the nodes */
                node.X += horizontalChange;
                node.Y += verticalChange;
            }
        }
    }
}
