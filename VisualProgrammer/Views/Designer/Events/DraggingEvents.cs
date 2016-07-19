﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VisualProgrammer.Views.Designer.Events
{
    public class DragDropEventArgs : RoutedEventArgs
    {
        private object draggedItem = null;
        private object returnItem = null;
        private bool cancel = false;

        public DragDropEventArgs(RoutedEvent routedEvent, object source, object draggedItem)
            :base(routedEvent, source)
        {
            this.draggedItem = draggedItem;
        }

        public object DraggedItem
        {
            get { return draggedItem; }
        }

        public object ReturnItem
        {
            get { return returnItem; }
            set
            {
                if (returnItem == value)
                    return;

                returnItem = value;
            }
        }

        public bool Cancel
        {
            get { return cancel; }
            set
            {
                if (cancel == value)
                    return;

                cancel = value;
            }
        }
    }

    public delegate void DragDropEventHandler(object sender, DragDropEventArgs e);
}
