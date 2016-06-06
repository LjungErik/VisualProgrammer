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
    public class SelectionDropDown : ContentControl
    {
        #region Private Data Members

        private ComboBox selectionCombobox = null;

        #endregion Private Data Members

        #region Dependency Property

        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(SelectionDropDown));

        #endregion

        public int Index
        {
            get
            {
                return (int)GetValue(IndexProperty);
            }
            set
            {
                SetValue(IndexProperty, value);
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
        }

        #region Private Methods

        static SelectionDropDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectionDropDown), new FrameworkPropertyMetadata(typeof(SelectionDropDown)));
        }

        #endregion
    }
}
