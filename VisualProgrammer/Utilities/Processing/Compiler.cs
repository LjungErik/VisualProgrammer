using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.Utilities.Processing.Commands.Compiler;
using VisualProgrammer.Utilities.Processing.Commands.Shell;
using VisualProgrammer.Utilities.Logger;
using VisualProgrammer.Enums;
using VisualProgrammer.Data.Actions;

namespace VisualProgrammer.Utilities.Processing
{
    public class Compiler
    {

        private List<string> dependencyDir = new List<string>();
        private List<string> dependencyName = new List<string>();

        private ICompilerCommand compilerCommand;

        private CompileLogger _logger;

        public Compiler(List<RobotAction> tasks, ICompilerCommand compiler, CompileLogger logger)
        { 
            //Set the dependencies
            SetUpDependencies(tasks);
            compilerCommand = compiler;
            _logger = logger;
        }

        public bool Execute(string outputFile)
        {
            try
            {
                _logger.WriteInfo("Creating dependency folder...");
                //Create needed dependency directory
                CommandShell.Execute("mkdir .dep");

                _logger.WriteInfo("Compiling main C-file...");

                //Execute command for main file
                CommandShell.Execute(compilerCommand.GetCompilerCommand(outputFile, "", dependencyDir.ToArray()));

                _logger.WriteInfo("Compiling dependency files:");

                //For each dependency execute the needed command
                for (int i = 0; i < dependencyDir.Count; i++)
                {
                    _logger.WriteInfo(" * Compiling " + dependencyDir[i] + "/" + dependencyName[i]);
                    CommandShell.Execute(compilerCommand.GetCompilerCommand(dependencyName[i], dependencyDir[i], dependencyDir.ToArray()));
                }

                _logger.WriteInfo("Linking compiled files...");
                //Link the files together
                CommandShell.Execute(compilerCommand.GetLinkCommand(outputFile, "", dependencyDir.ToArray(), dependencyName.ToArray()));

                _logger.WriteInfo("Generating .hex file...");
                //Create a .hex file from linked .elf file
                CommandShell.Execute(compilerCommand.GetHexCopyCommand(outputFile, ""));

                _logger.WriteInfo("Compiling complete");
                //No exceptions, everything went as planed
                return true;
            }
            catch(InvalidOperationException e)
            {
                _logger.WriteError(e.Message);
                _logger.SetStatus(StatusType.Error);
                //Failed to finish
                return false;
            }
            catch (Exception)
            {
                _logger.WriteError("Compiler experienced unknown error!");
                _logger.SetStatus(StatusType.Error);
                //Failed to finish
                return false;
            }
        }

        public void CleanUp()
        {
            try
            {
                _logger.WriteInfo("Performing cleanup...");

                CommandShell.Execute("rm -rf .dep");

                _logger.WriteInfo("Removed .dep folder with dependency files");
            }
            catch(InvalidOperationException e)
            {
                _logger.WriteWarning(e.Message);
            }
            catch(Exception)
            {
                _logger.WriteWarning("Clean up process experienced unknown error!");
            }
        }

        private void SetUpDependencies(List<RobotAction> tasks)
        {
            //Loop through all of the tasks and generate dependencies
            foreach (var task in tasks)
            {
                switch (task.GetType().Name)
                {
                    case "ServoMoveAction":
                        AddDepDirectory("LibraryFiles/Main_Library/MAIN_ROBOT");
                        AddDepName("RobotLib");
                        AddDepDirectory("LibraryFiles/Main_Library/SERVO");
                        AddDepName("ServoLib");
                        AddDepDirectory("LibraryFiles/Main_Library/UART");
                        AddDepName("UARTLib");
                        AddDepDirectory("LibraryFiles/Extended_Library/SERVO");
                        AddDepName("ServoExtended");
                        break;
                    case "UARTSendAction":
                        AddDepDirectory("LibraryFiles/Main_Library/MAIN_ROBOT");
                        AddDepName("RobotLib");
                        AddDepDirectory("LibraryFiles/Main_Library/UART");
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
