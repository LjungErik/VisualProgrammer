using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VisualProgrammer.Controls.Dropdowns
{
    public class MultiLineDropDown : ContentControl
    {
        #region Private Data Members

        private TextBox multiLineTextbox = null;

        #endregion Private Data Members

        #region Dependency Property

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MultiLineDropDown));

        #endregion

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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //
            // Cache the textbox of the visual tree that needs to be accessed later
            //
            this.multiLineTextbox = (TextBox)this.Template.FindName("PART_MultiLineTextBox", this);
            if(this.multiLineTextbox == null)
            {
                throw new ArgumentException("Failed to find 'PART_MultiLineTextBox' in the visual tree for 'MultiLineDropDown'");
            }

        }

        #region Private Methods

        static MultiLineDropDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiLineDropDown), new FrameworkPropertyMetadata(typeof(MultiLineDropDown)));
        }

        #endregion
    }
}
