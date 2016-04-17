using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrammer.Actions
{
    public class SleepAction : IRobotAction
    {
        //Sleep time in ms
        public int Time { get; set; }

        public SleepAction(int time)
        {
            this.Time = time;
        }

        public string GetActionType()
        {
            return "Sleep";
        }
    }
}
