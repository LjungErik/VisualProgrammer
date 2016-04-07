using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.Utilities;

namespace VisualProgrammer.ViewModels.Toolbox
{
    public class ToolboxViewModel : AbstractModelBase
    {
        #region Private Data Members

        private ImpObservableCollection<ToolboxItemViewModel> toolboxItem = null;

        #endregion Private Data Members

        public ToolboxViewModel()
        {
            PopulateToolbox();
        }

        public ImpObservableCollection<ToolboxItemViewModel> Tools
        {
            get { return toolboxItem; }
        }

        #region Private Methods

        private void PopulateToolbox()
        {
            toolboxItem = new ImpObservableCollection<ToolboxItemViewModel>();

            toolboxItem.Add(new ServoMoveToolboxItemViewModel());
            toolboxItem.Add(new UARTSendToolboxItemViewModel());
        }

        #endregion Private Methods
    }
}
