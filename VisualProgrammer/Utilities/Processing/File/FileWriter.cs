using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrammer.Utilities.Processing.File
{
    public class FileWriter : IFileWriter
    {
        private StreamWriter writer;

        public FileWriter()
        { }

        public void Open(string fileDir)
        {
            if(writer == null)
            {
                writer = new StreamWriter(fileDir);
            }
        }

        public void WriteLine(string text)
        {
            if (writer != null)
                writer.WriteLine(text);
        }

        public void Dispose()
        {
            if(writer != null)
            {
                writer.Close();
                writer = null;
            }
        }
    }
}
