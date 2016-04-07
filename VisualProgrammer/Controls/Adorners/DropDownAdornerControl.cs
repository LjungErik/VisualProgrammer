using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace VisualProgrammer.Controls.Adorners
{

    public abstract class DropDownAdornerControl : ContentControl
    {

        #region Dependency Properties and Routed Events

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(DropDownAdornerControl), 
                new FrameworkPropertyMetadata(default(string),FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly RoutedEvent OkButtonClickEvent =
            EventManager.RegisterRoutedEvent("OkButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DropDownAdornerControl));

        public static readonly RoutedEvent CancelButtonClickEvent =
            EventManager.RegisterRoutedEvent("CancelButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DropDownAdornerControl));

        #endregion Events

        #region Properties

        public string Value
        {
            get
            {
                return (string)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion Properties

        #region Events

        public event RoutedEventHandler OkButtonClick
        {
            add { AddHandler(OkButtonClickEvent, value); }
            remove { RemoveHandler(OkButtonClickEvent, value); }
        }

        public event RoutedEventHandler CancelButtonClick
        {
            add { AddHandler(CancelButtonClickEvent, value); }
            remove { RemoveHandler(CancelButtonClickEvent, value); }
        }

        #endregion Events

        /// <summary>
        /// Bring the Adorner into focus
        /// </summary>
        public abstract void Focused();
    }
}
