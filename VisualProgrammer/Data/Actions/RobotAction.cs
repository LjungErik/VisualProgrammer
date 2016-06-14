using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VisualProgrammer.Data.Actions
{
    [XmlInclude(typeof(SleepAction))]
    [XmlInclude(typeof(ServoMoveAction))]
    [XmlInclude(typeof(StartAction))]
    [XmlInclude(typeof(UARTSendAction))]
    public class RobotAction
    {
    }
}
