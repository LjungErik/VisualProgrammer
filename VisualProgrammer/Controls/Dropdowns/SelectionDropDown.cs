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
    public class SelectionDropDown : DropDownAdornerControl
    {
        #region Private Data Members

        private ComboBox selectionCombobox = null;

        private Canvas OkButton = null;

        private Canvas CancelButton = null;

        #endregion Private Data Members

        /// <summary>
        /// Toggles focus on the multiline textbox
        /// </summary>
        /// <param name="state">the state to toggle to</param>
        public override void Focused()
        {
            if(selectionCombobox != null)
            {
                selectionCombobox.Focus();
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //
            // Cache the textbox of the visual tree that needs to be accessed later
            //
            this.selectionCombobox = (ComboBox)this.Template.FindName("PART_SelectionComboBox", this);
            if(this.selectionCombobox == null)
            {
                throw new ArgumentException("Failed to find 'PART_SelectionComboBox' in the visual tree for 'SelectionDropDown'");
            }

            this.OkButton = (Canvas)this.Template.FindName("PART_OkButton", this);
            if(this.OkButton == null)
            {
                throw new ArgumentException("Failed to find 'PART_OkButton' in visual tree for 'SelectionDropDown'");
            }

            this.CancelButton = (Canvas)this.Template.FindName("PART_CancelButton", this);
            if(this.CancelButton == null)
            {
                throw new ArgumentException("Failed to find 'PART_CancelButton' in visual tree for 'SelectionDropDown'");
            }

            this.OkButton.MouseLeftButtonUp += new MouseButtonEventHandler(OkButton_Clicked);

            this.CancelButton.MouseLeftButtonUp += new MouseButtonEventHandler(CancelButton_Clicked);
        }

        #region Private Methods

        static SelectionDropDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectionDropDown), new FrameworkPropertyMetadata(typeof(SelectionDropDown)));
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
