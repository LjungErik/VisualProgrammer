using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

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
    * Modified on April 4 2016, by Erik Ljung
    */

    /// <summary>
    /// Abstract base for view-model classes that need to implement INotifyPropertyChanged.
    /// </summary>
    public abstract class AbstractModelBase : INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
