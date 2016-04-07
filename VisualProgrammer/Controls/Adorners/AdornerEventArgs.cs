using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace VisualProgrammer.Controls.Adorners
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

    public class AdornerEventArgs : RoutedEventArgs
    {
        private FrameworkElement adorner = null;

        public AdornerEventArgs(RoutedEvent routedEvent, object source, FrameworkElement adorner) :
            base(routedEvent, source)
        {
            this.adorner = adorner;
        }

        public FrameworkElement Adorner
        {
            get
            {
                return adorner;
            }
        }
    }

    public delegate void AdornerEventHandler(object sender, AdornerEventArgs e);
}
