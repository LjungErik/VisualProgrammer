using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisualProgrammer.ViewModels.Designer
{
    /* 
    * Copyright (c) 2012 Ashley Davis
    * --------------------------------------------------
    * Derived and Adapted from Ashley Davis article
    * "NetworkView: A WPF custom control for 
    * visualizing and editing networks, graphs 
    * and flow-charts".
    * --------------------------------------------------
    * This code was created by Ashley Davis, 2 Aug 2012
    * Licenced under the CPOL-License which is available
    * at the root of this project.
    * --------------------------------------------------
    */

    /// <summary>
    /// Defines the type of a connector (aka connection point).
    /// </summary>
    public enum ConnectorType
    {
        Undefined,
        Input,
        Output,
    }
}
