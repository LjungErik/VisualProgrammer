using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.Utilities.Logger;

namespace VisualProgrammer.Utilities.Processing.Commands.Shell
{
    public class CommandShell
    {

        public static void Execute(string command) 
        {
            Process cmdProcess = new Process();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command;

            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;

            startInfo.WorkingDirectory = Environment.CurrentDirectory;

            cmdProcess.StartInfo = startInfo;

            cmdProcess.Start();

            //Wait for possible errors
            string error = cmdProcess.StandardError.ReadToEnd();

            cmdProcess.WaitForExit(5000);

            if (!String.IsNullOrEmpty(error))
            {
                //Trigger a execpetion to signal error to caller
                throw new InvalidOperationException(error);
            }
        }
    }
}
