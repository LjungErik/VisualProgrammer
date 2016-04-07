using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VisualProgrammer.Controls.Adorners;

namespace VisualProgrammer.Controls.Dropdowns
{
    public class MultiLineDropDown : DropDownAdornerControl
    {
        #region Private Data Members

        private TextBox multiLineTextbox = null;

        private Canvas OkButton = null;

        private Canvas CancelButton = null;

        #endregion Private Data Members

        /// <summary>
        /// Toggles focus on the multiline textbox
        /// </summary>
        /// <param name="state">the state to toggle to</param>
        public override void Focused()
        {
            if(multiLineTextbox != null)
            {
                multiLineTextbox.Focus();
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

            this.OkButton = (Canvas)this.Template.FindName("PART_OkButton", this);
            if (this.OkButton == null)
            {
                throw new ArgumentException("Failed to find 'PART_OkButton' in visual tree for 'MultiLineDropDown'");
            }

            this.CancelButton = (Canvas)this.Template.FindName("PART_CancelButton", this);
            if (this.CancelButton == null)
            {
                throw new ArgumentException("Failed to find 'PART_CancelButton' in visual tree for 'MultiLineDropDown'");
            }

            this.OkButton.MouseLeftButtonUp += new MouseButtonEventHandler(OkButton_Clicked);

            this.CancelButton.MouseLeftButtonUp += new MouseButtonEventHandler(CancelButton_Clicked);

        }

        #region Private Methods

        static MultiLineDropDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiLineDropDown), new FrameworkPropertyMetadata(typeof(MultiLineDropDown)));
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
