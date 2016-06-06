using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VisualProgrammer.Views.Designer;

namespace VisualProgrammer.Factory.MouseActions
{
    public interface IMouseAction
    {
        Cursor MouseCursor { get; }

        void OnMouseDown(DesignView designView, MouseEventArgs e);

        void OnMouseMove(DesignView designView, MouseEventArgs e);

        void OnMouseUp(DesignView designView, MouseEventArgs e);
    }
}
