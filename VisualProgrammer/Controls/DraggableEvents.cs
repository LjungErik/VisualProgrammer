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
        private bool failed = false;

        public DraggableDropEventArgs() { }

        public bool Failed
        {
            get { return failed; }
            set
            {
                if (failed == value)
                    return;

                failed = value;
            }
        }
    }

    public delegate void DraggableDropEventHandler(object sender, DraggableDropEventArgs e);
}
