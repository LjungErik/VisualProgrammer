using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VisualProgrammer.Controls.Dropdowns
{
    public class NumberDropDown : ContentControl
    {
        #region Private Data Members

        private TextBox numberTextbox = null;

        #endregion Private Data Members

        #region Dependency Property

        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register("Number", typeof(int), typeof(NumberDropDown));

        #endregion

        public int Number
        {
            get 
            { 
                return (int)GetValue(NumberProperty); 
            }
            set
            {
                SetValue(NumberProperty, value);
            }
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //
            // Cache the textbox of the visual tree that needs to be accessed later
            //
            this.numberTextbox = (TextBox)this.Template.FindName("PART_NumberTextBox", this);
            if (this.numberTextbox == null)
            {
                throw new ArgumentException("Failed to find 'PART_NumberTextBox' in the visual tree for 'NumberDropDown'");
            }

            this.numberTextbox.PreviewTextInput += new TextCompositionEventHandler(NumberTextbox_PreviewTextInput);
        }

        #region Private Methods

        static NumberDropDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberDropDown), new FrameworkPropertyMetadata(typeof(NumberDropDown)));
        }

        /// <summary>
        /// Tests that the input is only numeric
        /// </summary>
        private void NumberTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]+");
            /* Only allow numbers */
            e.Handled = !regex.IsMatch(e.Text);
        }

        #endregion

    }
}
