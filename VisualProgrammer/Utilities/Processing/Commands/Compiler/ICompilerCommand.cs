using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrammer.Utilities.Processing.Commands.Compiler
{
    public interface ICompilerCommand
    {
        string GetCompilerCommand(string file, string fileDir, string[] depFolders);

        string GetLinkCommand(string file, string fileDir, string[] depFolders, string[] depFileNames);

        string GetHexCopyCommand(string file, string fileDir);
    }
}
