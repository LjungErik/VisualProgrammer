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
    public class SliderDropDown : ContentControl
    {
        #region Private Data Members

        private TextBox sliderValueTextbox = null;

        private Slider slider = null;

        #endregion Private Data Members

        #region Dependency Property

        public static readonly DependencyProperty SliderProperty =
            DependencyProperty.Register("Slider", typeof(int), typeof(SliderDropDown));

        #endregion

        public int Slider
        {
            get 
            { 
                return (int)GetValue(SliderProperty); 
            }
            set
            {
                SetValue(SliderProperty, value);
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

            this.sliderValueTextbox.PreviewTextInput += new TextCompositionEventHandler(SliderValueTextbox_PreviewTextInput);
        }

        #region Private Methods

        static SliderDropDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SliderDropDown), new FrameworkPropertyMetadata(typeof(SliderDropDown)));
        }

        /// <summary>
        /// Tests that the input is only numeric
        /// </summary>
        private void SliderValueTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]+");
            /* Only allow numbers */
            e.Handled = !regex.IsMatch(e.Text);
        }

        #endregion

    }
}
