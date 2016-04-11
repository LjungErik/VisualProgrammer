using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.Actions;
using VisualProgrammer.Enums;
using VisualProgrammer.Processing.Commands.Compiler;
using VisualProgrammer.Processing.File;
using VisualProgrammer.Utilities;
using VisualProgrammer.Utilities.Logger;
using VisualProgrammer.ViewModels.Designer;

namespace VisualProgrammer.Processing
{
    public class Builder
    {
        /// <summary>
        /// Build the program
        /// </summary>
        public static void PerformBuild(CompileLogger logger, StartNodeViewModel startNode)
        {
            //Make sure that the node exists and can be used
            //When compiling
            if(startNode == null)
            {
                logger.WriteError("No start node was found, could not begin compilation.");
                logger.SetStatus(StatusType.Error);
                return;
            }

            List<IRobotAction> actions = ModelCollector.GetModels(startNode);

            logger.SetStatus(StatusType.Processing);

            logger.SetProgress(8);

            Parser parser = new Parser(new FileWriter(), logger);
            parser.GenrateCode(actions, "Testname.c");

            logger.SetProgress(35);

            Compiler compiler = new Compiler(actions,
                                             new CompilerCommand(),
                                             logger);

            logger.SetProgress(45);

            if (compiler.Execute("Testname"))
            {
                logger.SetProgress(85);
                compiler.CleanUp();
                logger.SetStatus(StatusType.Finished);
            }
            else
            {
                compiler.CleanUp();
            }

            logger.SetProgress(100);
        }
    }
}
