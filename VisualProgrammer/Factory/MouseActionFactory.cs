using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammer.Enums;
using VisualProgrammer.Factory.MouseActions;

namespace VisualProgrammer.Factory
{
    public class MouseActionFactory
    {
        #region Private Data Members

        private SelectMouseAction selectAction = null;

        private MoveMouseAction moveAction = null;

        private ArrowMouseAction arrowAction = null;

        #endregion Private Data Members

        public MouseActionFactory()
        {
            selectAction = new SelectMouseAction();
            moveAction = new MoveMouseAction();
            arrowAction = new ArrowMouseAction();
        }

        public IMouseAction GetAction(MouseAction action)
        {
            switch (action)
            {
                case MouseAction.Select:
                    return selectAction;
                case MouseAction.Move:
                    return moveAction;
                default:
                    return arrowAction;
            }
        }
    }
}
