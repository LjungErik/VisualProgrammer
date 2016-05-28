using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrammer.Controls
{
    public interface IDropView
    {
        void DragOver(IDraggable dragged);

        void Dropped(IDraggable dragged);
    }
}
