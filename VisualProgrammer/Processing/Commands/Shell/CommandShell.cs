using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrammer.Processing.Commands.Shell
{
    public class CommandShell : ICommandShell
    {
        private Process cmdProcess;

        public CommandShell() { }

        public void Open() 
        {
            if (cmdProcess == null)
            {
                cmdProcess = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.FileName = "cmd.exe";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                cmdProcess.StartInfo = startInfo;
                cmdProcess.Start();
            }
        }

        public void Execute(string command) 
        {
            if (cmdProcess != null)
            {
                Debug.WriteLine(command);

                cmdProcess.StandardInput.WriteLine(command);
                cmdProcess.StandardInput.Flush();
            }
        }

        public void Close() 
        {
            if (cmdProcess != null)
            {
                cmdProcess.CloseMainWindow();
                cmdProcess.Close();
                cmdProcess = null;
            }
        }
    }
}
