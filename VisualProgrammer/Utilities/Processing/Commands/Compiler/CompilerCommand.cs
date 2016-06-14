using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrammer.Utilities.Processing.Commands.Compiler
{
    public class CompilerCommand : ICompilerCommand
    {
        /* Compiler/Linker options */
        private const string COMPILER           = "avr-gcc";
        private const string MMCU               = "-mmcu=atmega64";
        private const string DEBUG              = "-gdwarf-2";
        private const string DF_CPU             = "-DF_CPU=16000000UL";
        private const string OPTIM_OPTION       = "-Os";
        private const string CODE_CONVENTION    = "-funsigned-char -funsigned-bitfields -fpack-struct -fshort-enums";
        private const string WARNING_FLAGS      = "-Wall -Wstrict-prototypes";
        private const string LANGUAGE_STANDARD  = "-std=c99";
        private const string DEPENDENCY         = "-MMD -MP -MF";
        private const string DEP_DIR            = ".dep/";

        /* Linker options */
        private const string OUTPUT             = "--output";
        private const string MATH_LIB           = "-lm";

        /* Hex creator options*/
        private const string OBJCOPY            = "avr-objcopy";
        private const string OUTPUT_FORMAT      = "ihex";
        private const string REMOVED_SECTIONS   = "-R .eeprom -R .fuse -R .lock";

        private string currentDirectory = ".";

        public CompilerCommand()
        { }

        public CompilerCommand(string currentDir)
        {
            currentDirectory = currentDir;
        }

        public string GetCompilerCommand(string file, string fileDir, string[] depFolders)
        {
            string cmd = String.Format("{0} -c {1} -I{2} {3} {4} {5} {6} {7}",
                                    COMPILER,
                                    MMCU,
                                    currentDirectory,
                                    DEBUG,
                                    DF_CPU,
                                    OPTIM_OPTION,
                                    CODE_CONVENTION,
                                    WARNING_FLAGS);

            //Add dependency files (make with -I to indicate current directory)
            if(depFolders.Length > 0)
                cmd += " -I" + String.Join(" -I", depFolders);
            //Add language standard
            cmd += " " + LANGUAGE_STANDARD;
            //Add dependency
            cmd += " " + DEPENDENCY + " " + DEP_DIR + file + ".o.d ";
            //Add file to compile + output file
            string fullFileName = String.IsNullOrEmpty(fileDir) ? file : (fileDir + "/" + file);

            cmd += fullFileName + ".c -o " + fullFileName + ".o";

            return cmd;
        }

        public string GetLinkCommand(string file, string fileDir, string[] depFolders, string[] depFileNames)
        {
            string cmd = String.Format("{0} {1} -I{2} {3} {4} {5} {6} {7}",
                                    COMPILER,
                                    MMCU,
                                    currentDirectory,
                                    DEBUG,
                                    DF_CPU,
                                    OPTIM_OPTION,
                                    CODE_CONVENTION,
                                    WARNING_FLAGS);

            //Add dependency files (make with -I to indicate current directory)
            cmd += " -I" + String.Join(" -I", depFolders);
            //Add language standard
            cmd += " " + LANGUAGE_STANDARD;
            //Add dependency
            cmd += " " + DEPENDENCY + " " + DEP_DIR + file + ".elf.d ";
            //Add file to compile + output file
            string fullFileName = String.IsNullOrEmpty(fileDir) ? file : (fileDir + "/" + file);

            cmd += fullFileName + ".o";

            //Add dependencyfiles (object files)
            for (int i = 0; i < depFolders.Length; i ++)
            {
                cmd += " " + depFolders[i] + "/" + depFileNames[i] + ".o";
            }

            cmd += " " + OUTPUT + " " + fullFileName + ".elf " + MATH_LIB; 

            return cmd;
        }

        public string GetHexCopyCommand(string file, string fileDir)
        {
            string cmd = String.Format("{0} -O {1} {2}",
                                   OBJCOPY,
                                   OUTPUT_FORMAT,
                                   REMOVED_SECTIONS);

            string fullFileName = String.IsNullOrEmpty(fileDir) ? file : (fileDir + "/" + file);

            //Add the elf file
            cmd += " " + fullFileName + ".elf";
            //Add output file .hex
            cmd += " " + fullFileName + ".hex";

            return cmd;
        }
    }
}
