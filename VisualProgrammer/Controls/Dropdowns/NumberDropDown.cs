using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VisualProgrammer.Controls.Adorners;

namespace VisualProgrammer.Controls.Dropdowns
{
    public class NumberDropDown : DropDownAdornerControl
    {
        #region Private Data Members

        private TextBox numberTextbox = null;

        private Canvas OkButton = null;

        private Canvas CancelButton = null;

        #endregion Private Data Members

        /// <summary>
        /// Focus on the number textbox
        /// </summary>
        public override void Focused()
        {
            if (numberTextbox != null)
            {
                numberTextbox.Focus();
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

            this.OkButton = (Canvas)this.Template.FindName("PART_OkButton", this);
            if (this.OkButton == null)
            {
                throw new ArgumentException("Failed to find 'PART_OkButton' in visual tree for 'NumberDropDown'");
            }

            this.CancelButton = (Canvas)this.Template.FindName("PART_CancelButton", this);
            if (this.CancelButton == null)
            {
                throw new ArgumentException("Failed to find 'PART_CancelButton' in visual tree for 'NumberDropDown'");
            }

            this.numberTextbox.PreviewTextInput += new TextCompositionEventHandler(NumberTextbox_PreviewTextInput);

            this.OkButton.MouseLeftButtonUp += new MouseButtonEventHandler(OkButton_Clicked);

            this.CancelButton.MouseLeftButtonUp += new MouseButtonEventHandler(CancelButton_Clicked);
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

        private void OkButton_Clicked(object sender, MouseButtonEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DropDownAdornerControl.OkButtonClickEvent));
        }

        private void CancelButton_Clicked(object sender, MouseButtonEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DropDownAdornerControl.CancelButtonClickEvent));
        }

        #endregion

    }
}
