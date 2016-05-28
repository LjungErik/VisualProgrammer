using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VisualProgrammer.Views.Restructure.Designer
{
    public partial class DesignView
    {
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
 	         base.OnMouseDown(e);

            // Clear all selected nodes
             nodeControl.SelectedItems.Clear();

             if (MouseHandler != null)
                 MouseHandler.OnMouseDown(this, e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (MouseHandler != null)
                MouseHandler.OnMouseMove(this, e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (MouseHandler != null)
                MouseHandler.OnMouseUp(this, e);
        }
    }
}
