using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.Utilities.Processing.FileHelpers;
using VisualProgrammer.Utilities.Logger;
using VisualProgrammer.Enums;
using VisualProgrammer.Data.Actions;

namespace VisualProgrammer.Utilities.Processing
{
    

    public class Parser
    {
        private const int UP_START_TIME = 3000;

        private List<string> dependencies;
        private List<string> preConditions;
        private List<string> postConditions;
        private List<string> taskCalls;

        private IFileWriter _fileWriter;

        private CompileLogger _logger;

        public Parser(IFileWriter fileWriter, CompileLogger logger)
        { 
            dependencies = new List<string>();
            preConditions = new List<string>();
            postConditions = new List<string>();
            taskCalls = new List<string>();

            _fileWriter = fileWriter;
            _logger = logger;

            /* Default Dependencys and tasks */
            AddDependency("#include <util/delay.h>");
            AddTaskCall("_delay_ms(" + UP_START_TIME + ");");
        }

        public bool GenrateCode(List<RobotAction> actions, string outputFile)
        {
            bool ret = true;
            //Open the writer on the correect file
            _fileWriter.Open(outputFile);

            _logger.WriteInfo("Begin parsing of robot actions...");

            foreach (var action in actions)
            {
                switch (action.GetType().Name)
                {
                    case "ServoMoveAction":
                        ServoMoveAction move = (ServoMoveAction)action;
                        AddDependency("#include \"ServoExtended.h\"");
                        AddPreCondition("StartServo();");
                        AddTaskCall(String.Format("MoveDegrees({0},{1});", move.Servo, move.Degrees));
                        AddPostCondition("StopServo();");
                        break;
                    case "UARTSendAction":
                        UARTSendAction send = (UARTSendAction)action;
                        AddDependency("#include \"UARTLib.h\"");
                        AddPreCondition("InitUART();");
                        AddTaskCall(String.Format("Write(\"{0}\");", send.Message));
                        break;
                    case "SleepAction":
                        SleepAction sleep = (SleepAction)action;
                        AddDependency("#include <util/delay.h>");
                        AddTaskCall(String.Format("_delay_ms({0});", sleep.Time));
                        break;
                    default:
                        _logger.WriteError("Invalid action type, " + action.GetType());
                        _logger.SetStatus(StatusType.Error);
                        ret = false;
                        break;
                }
            }

            if (ret)
            {
                _logger.WriteInfo("Generating appropriate C-file...");

                WriteData(_fileWriter);

                _logger.WriteInfo("C-file generated");
            }

            //Dispose of the writer
            _fileWriter.Dispose();
            ClearLists();

            return ret;
        }

        private  void AddDependency(string dep)
        {
            //Only need one of each dependency
            if(!dependencies.Contains(dep))
                dependencies.Add(dep);
        }

        private  void AddPreCondition(string pre)
        {
            //Only need one pre-condition
            if(!preConditions.Contains(pre))
                preConditions.Add(pre);
        }

        private  void AddPostCondition(string post)
        {
            //Only need one post-condition
            if(!postConditions.Contains(post))
                postConditions.Add(post);
        }

        private  void AddTaskCall(string task)
        {
            taskCalls.Add(task);
        }

        private void WriteData(IFileWriter file)
        {
            foreach (var dependency in dependencies)
            {
                file.WriteLine(dependency);
            }

            file.WriteLine("int main(void)");
            file.WriteLine("{");

            foreach (var pre in preConditions)
            {
                file.WriteLine("\t" + pre);
            }

            foreach (var task in taskCalls)
            {
                file.WriteLine("\t" + task);
            }

            /* Default code to prevent robot from turning off directly */
            file.WriteLine("\twhile(1);"); //Infinity loop to continue execution
            

            foreach (var post in postConditions)
            {
                file.WriteLine("\t" + post);
            }

            file.WriteLine("}");
            
        }

        private void ClearLists()
        {
            dependencies.Clear();
            preConditions.Clear();
            postConditions.Clear();
            taskCalls.Clear();
        }
    }
}
