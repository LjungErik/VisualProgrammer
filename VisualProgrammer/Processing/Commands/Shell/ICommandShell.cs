using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrammer.Processing.Commands.Shell
{
    public interface ICommandShell
    {
        void Open();

        void Execute(string command);

        void Close();
    }
}
