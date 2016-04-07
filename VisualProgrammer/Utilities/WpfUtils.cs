using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;

namespace VisualProgrammer.Utilities
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
    /// This class contains helper functions for dealing with WPF.
    /// </summary>
    public static class WpfUtils
    {

        public static ParentT FindVisualParentWithType<ParentT>(FrameworkElement childElement)
            where ParentT : class
        {
            FrameworkElement parentElement = (FrameworkElement)VisualTreeHelper.GetParent(childElement);
            if (parentElement != null)
            {
                ParentT parent = parentElement as ParentT;
                if (parent != null)
                {
                    return parent;
                }

                return FindVisualParentWithType<ParentT>(parentElement);
            }

            return null;
        }
    }
}
