using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrammer.Utilities.Processing.FileHelpers
{
    public interface IFileHandler
    {
        void Save<T>(string filepath, T data);

        T Load<T>(string filepath);
    }
}
