using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VisualProgrammer.Utilities.Commands
{
    public class CustomCommands
    {
        private static RoutedUICommand build;

        static CustomCommands()
        {
            build = new RoutedUICommand("Build", "Build", typeof(CustomCommands));
        }

        public static RoutedUICommand Build
        {
            get { return build; }
        }

    }
}
