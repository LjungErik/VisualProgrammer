using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;

namespace VisualProgrammer.Utilities
{
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
