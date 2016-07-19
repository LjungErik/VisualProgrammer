using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrammer.Controls
{
    public class DraggableDragEventArgs : EventArgs
    {
        public DraggableDragEventArgs() { } 
    }

    public delegate void DraggableDragEventHandler(object sender, DraggableDragEventArgs e);

    public class DraggableDropEventArgs : DraggableDragEventArgs
    {
        public DraggableDropEventArgs() { }
    }

    public delegate void DraggableDropEventHandler(object sender, DraggableDropEventArgs e);
}
