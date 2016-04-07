using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrammer.Processing.File
{
    public interface IFileWriter : IDisposable
    {
        void Open(string fileDir);

        void WriteLine(string text);
    }
}
