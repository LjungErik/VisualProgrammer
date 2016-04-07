using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.Processing.Commands.Compiler;
using VisualProgrammer.Processing.Commands.Shell;
using VisualProgrammer.Actions;

namespace VisualProgrammer.Processing
{
    public class Compiler
    {

        private List<string> dependencyDir = new List<string>();
        private List<string> dependencyName = new List<string>();

        private ICommandShell commandShell;
        private ICompilerCommand compilerCommand;

        public Compiler(List<IRobotAction> tasks, ICommandShell shell, ICompilerCommand compiler)
        { 
            //Set the dependencies
            SetUpDependencies(tasks);
            commandShell = shell;
            compilerCommand = compiler;
        }

        public void Execute(string outputFile)
        {
            //Set up command prompt window (should be invisible for user)
            commandShell.Open();

            //Create needed dependency directory
            commandShell.Execute("mkdir .dep");

            //Execute command for main file
            commandShell.Execute(compilerCommand.GetCompilerCommand(outputFile, "", dependencyDir.ToArray()));

            //For each dependency execute the needed command
            for (int i = 0; i < dependencyDir.Count; i++)
            {
                commandShell.Execute(compilerCommand.GetCompilerCommand(dependencyName[i], dependencyDir[i], dependencyDir.ToArray()));
            }
            
            //Link the files together
            commandShell.Execute(compilerCommand.GetLinkCommand(outputFile, "", dependencyDir.ToArray(), dependencyName.ToArray()));
            
            //Create a .hex file from linked .elf file
            commandShell.Execute(compilerCommand.GetHexCopyCommand(outputFile, ""));
            //Close command prompt window

            commandShell.Close();
        }

        private void SetUpDependencies(List<IRobotAction> tasks)
        {
            //Loop through all of the tasks and generate dependencies
            foreach (var task in tasks)
            {
                switch (task.GetActionType())
                {
                    case "ServoMove":
                        AddDepDirectory("Main_Library/MAIN_ROBOT");
                        AddDepName("RobotLib");
                        AddDepDirectory("Main_Library/SERVO");
                        AddDepName("ServoLib");
                        AddDepDirectory("Main_Library/UART");
                        AddDepName("UARTLib");
                        AddDepDirectory("Extended_Library/SERVO");
                        AddDepName("ServoExtended");
                        break;
                    case "UARTSend":
                        AddDepDirectory("Main_Library/MAIN_ROBOT");
                        AddDepName("RobotLib");
                        AddDepDirectory("Main_Library/UART");
                        AddDepName("UARTLib");
                        break;
                }
            }
        }

        private void AddDepDirectory(string dependency)
        {
            if (!dependencyDir.Contains(dependency))
                dependencyDir.Add(dependency);
        }

        private void AddDepName(string dependency)
        {
            if (!dependencyName.Contains(dependency))
                dependencyName.Add(dependency);
        }
    }
}
