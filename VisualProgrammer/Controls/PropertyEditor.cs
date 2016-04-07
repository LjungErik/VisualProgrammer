using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace VisualProgrammer.Controls
{
    public class PropertyEditor : ContentControl
    {
        #region Dependency Properties/Event Definitions

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(PropertyEditor),
                new FrameworkPropertyMetadata(""));

        #endregion Dependency Properties/Event Definitions

        public PropertyEditor()
        { }

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        #region Private Method

        /// <summary>
        /// Static constructor.
        /// </summary>
        static PropertyEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyEditor), new FrameworkPropertyMetadata(typeof(PropertyEditor)));
        }

        #endregion Private Method
    }
}
