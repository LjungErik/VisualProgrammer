using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.ViewModels.Designer;

namespace VisualProgrammer.ViewModels.Toolbox
{
    public abstract class ToolboxItemViewModel
    {
        public virtual string ImageSource { get; set; }

        public abstract NodeViewModel GetNode();
    }
}
