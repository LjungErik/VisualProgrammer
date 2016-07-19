using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrammer.Utilities
{
    public interface IChangeable
    {
        event PropertyChangedEventHandler ChangeMade;
    }
}
