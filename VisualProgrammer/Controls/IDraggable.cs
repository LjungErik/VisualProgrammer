using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrammer.Controls
{
    public interface IDraggable
    {
        event DraggableDragEventHandler OnDragging;

        event DraggableDropEventHandler OnDragCompleted;

        void Drop();
    }
}
