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
    public class ArrowMouseAction : IMouseAction
    {
        public Cursor MouseCursor
        {
            get { return Cursors.Arrow; }
        }

        public void OnMouseDown(DesignView designView, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void OnMouseMove(DesignView designView, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void OnMouseUp(DesignView designView, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
