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
    public class SliderDropDown : DropDownAdornerControl
    {
        #region Private Data Members

        private TextBox sliderValueTextbox = null;

        private Slider slider = null;

        private Canvas OkButton = null;

        private Canvas CancelButton = null;

        #endregion Private Data Members

        /// <summary>
        /// Focus on the slider
        /// </summary>
        public override void Focused()
        {
            if(slider != null)
            {
                slider.Focus();
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //
            // Cache the textbox of the visual tree that needs to be accessed later
            //
            this.slider = (Slider)this.Template.FindName("PART_Slider", this);
            if(this.slider == null)
            {
                throw new ArgumentException("Failed to find 'PART_Slider' in the visual tree for 'SliderDropDown'");
            }

            this.sliderValueTextbox = (TextBox)this.Template.FindName("PART_SliderValueTextbox", this);
            if(this.sliderValueTextbox == null)
            {
                throw new ArgumentException("Failed to find 'PART_SliderValueTextbox' in the visual tree for 'SliderDropDown'");
            }

            this.OkButton = (Canvas)this.Template.FindName("PART_OkButton", this);
            if (this.OkButton == null)
            {
                throw new ArgumentException("Failed to find 'PART_OkButton' in visual tree for 'SliderDropDown'");
            }

            this.CancelButton = (Canvas)this.Template.FindName("PART_CancelButton", this);
            if (this.CancelButton == null)
            {
                throw new ArgumentException("Failed to find 'PART_CancelButton' in visual tree for 'SliderDropDown'");
            }

            this.OkButton.MouseLeftButtonUp += new MouseButtonEventHandler(OkButton_Clicked);

            this.CancelButton.MouseLeftButtonUp += new MouseButtonEventHandler(CancelButton_Clicked);
        }

        #region Private Methods

        static SliderDropDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SliderDropDown), new FrameworkPropertyMetadata(typeof(SliderDropDown)));
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
