using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.Processing.File;
using VisualProgrammer.Actions;

namespace VisualProgrammer.Processing
{
    public class Parser
    {
        private List<string> dependencies;
        private List<string> preConditions;
        private List<string> postConditions;
        private List<string> taskCalls;

        private IFileWriter _fileWriter;

        public Parser(IFileWriter fileWriter)
        { 
            dependencies = new List<string>();
            preConditions = new List<string>();
            postConditions = new List<string>();
            taskCalls = new List<string>();

            _fileWriter = fileWriter;
        }

        public void GenrateCode(List<IRobotAction> tasks, string outputFile)
        {
            //Open the writer on the correect file
            _fileWriter.Open(outputFile);

            foreach (var task in tasks)
            {
                switch (task.GetActionType())
                {
                    case "ServoMove":
                        ServoMoveAction move = (ServoMoveAction)task;
                        AddDependency("#include \"ServoExtended.h\"");
                        AddPreCondition("StartServo();");
                        AddTaskCall(String.Format("MoveDegrees({0},{1});", move.Servo, move.Degrees));
                        AddPostCondition("StopServo();");
                        break;
                    case "UARTSend":
                        UARTSendAction send = (UARTSendAction)task;
                        AddDependency("#include \"UARTLib.h\"");
                        AddPreCondition("InitUART();");
                        AddTaskCall(String.Format("Write(\"{0}\");", send.Message));
                        break;
                }
            }

            WriteData(_fileWriter);
            //Dispose of the writer
            _fileWriter.Dispose();
            ClearLists();
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
