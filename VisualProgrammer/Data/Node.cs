using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VisualProgrammer.Data.Actions;

namespace VisualProgrammer.Data
{
    public class Node
    {
        public string NodeGuid;

        public RobotAction Action;

        public double XPos;

        public double YPos;

        public int ZIndex;
    }
}
