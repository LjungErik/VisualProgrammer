using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.ViewModels.Designer;

namespace VisualProgrammer.ViewModels.Toolbox
{
    public class ServoMoveToolboxItemViewModel : ToolboxItemViewModel
    {
        #region CONSTANTS

        private const string IMAGE_SOURCE = "pack://application:,,,/Resources/Images/toolbox_ServoMovenode.png";

        #endregion CONSTANTS

        public override string ImageSource
        {
            get
            {
                return IMAGE_SOURCE;
            }
        }

        public override NodeViewModel GetNode()
        {
            return new ServoMoveNodeViewModel();
        }
    }
}
