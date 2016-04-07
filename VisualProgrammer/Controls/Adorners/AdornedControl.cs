using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Diagnostics;

namespace VisualProgrammer.Controls.Adorners
{
    /* 
    * Copyright (c) 2012 Ashley Davis
    * --------------------------------------------------
    * Derived and Adapted from Ashley Davis article
    * "NetworkView: A WPF custom control for 
    * visualizing and editing networks, graphs 
    * and flow-charts".
    * --------------------------------------------------
    * This code was created by Ashley Davis, 2 Aug 2012
    * Licenced under the CPOL-License which is available
    * at the root of this project.
    * --------------------------------------------------
    * Modified on April 4 2016, by Erik Ljung
    */

    /// <summary>
    /// A content control that allows an adorner for the content to
    /// be defined in XAML.
    /// </summary>
    public class AdornedControl : ContentControl
    {
        #region Dependency Properties / Event Definitions

        /// <summary>
        /// Dependency properties.
        /// </summary>
        public static readonly DependencyProperty IsAdornerVisibleProperty =
            DependencyProperty.Register("IsAdornerVisible", typeof(bool), typeof(AdornedControl),
                new FrameworkPropertyMetadata(IsAdornerVisible_PropertyChanged));

        public static readonly DependencyProperty AdornerContentProperty =
            DependencyProperty.Register("AdornerContent", typeof(DropDownAdornerControl), typeof(AdornedControl),
                new FrameworkPropertyMetadata(AdornerContent_PropertyChanged));

        public static readonly DependencyProperty HorizontalAdornerPlacementProperty =
            DependencyProperty.Register("HorizontalAdornerPlacement", typeof(AdornerPlacement), typeof(AdornedControl),
                new FrameworkPropertyMetadata(AdornerPlacement.Inside));

        public static readonly DependencyProperty VerticalAdornerPlacementProperty =
            DependencyProperty.Register("VerticalAdornerPlacement", typeof(AdornerPlacement), typeof(AdornedControl),
                new FrameworkPropertyMetadata(AdornerPlacement.Inside));

        public static readonly DependencyProperty FadeInTimeProperty =
            DependencyProperty.Register("FadeInTime", typeof(double), typeof(AdornedControl),
                new FrameworkPropertyMetadata(0.25));

        public static readonly DependencyProperty FadeOutTimeProperty =
            DependencyProperty.Register("FadeOutTime", typeof(double), typeof(AdornedControl),
                new FrameworkPropertyMetadata(0.25));

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(string), typeof(AdornedControl),
                new FrameworkPropertyMetadata(""));

        public static readonly RoutedEvent AdornerShownEvent =
            EventManager.RegisterRoutedEvent("AdornerShown", RoutingStrategy.Bubble, typeof(AdornerEventHandler), typeof(AdornedControl));

        public static readonly RoutedEvent AdornerHiddenEvent =
            EventManager.RegisterRoutedEvent("AdornerHidden", RoutingStrategy.Bubble, typeof(AdornerEventHandler), typeof(AdornedControl));

        #endregion Dependency Properties / Event Definitions

        public AdornedControl()
        {
            this.DataContextChanged += new DependencyPropertyChangedEventHandler(AdornedControl_DataContextChanged);
        }

        /// <summary>
        /// Show the adorner.
        /// </summary>
        public void ShowAdorner()
        {
            IsAdornerVisible = true;

            adornerShowState = AdornerShowState.Visible;
        }

        /// <summary>
        /// Hide the adorner.
        /// </summary>
        public void HideAdorner()
        {
            IsAdornerVisible = false;

            adornerShowState = AdornerShowState.Hidden;
        }

        /// <summary>
        /// Fade the adorner in and make it visible.
        /// </summary>
        public void FadeInAdorner()
        {
            
            if (adornerShowState == AdornerShowState.Visible ||
                adornerShowState == AdornerShowState.FadingIn)
            {
                return;
            }

            this.ShowAdorner();

            if (adornerShowState != AdornerShowState.FadingOut)
            {
                adorner.Opacity = 0.0;
            }

            DoubleAnimation doubleAnimation = new DoubleAnimation(1.0, new Duration(TimeSpan.FromSeconds(FadeInTime)));
            doubleAnimation.Completed += new EventHandler(fadeInAnimation_Completed);
            doubleAnimation.Freeze(); // <-- Prevent editing during fadein

            // Start the animation that changes the Opacity of the adorener

            adorner.BeginAnimation(FrameworkElement.OpacityProperty, doubleAnimation);

            adornerShowState = AdornerShowState.FadingIn;
        }

        /// <summary>
        /// Fade the adorner out and make it visible.
        /// </summary>
        public void FadeOutAdorner()
        {
            if (adornerShowState == AdornerShowState.Hidden ||
                adornerShowState == AdornerShowState.FadingOut)
            {
                return;
            }

            DoubleAnimation fadeOutAnimation = new DoubleAnimation(0.0, new Duration(TimeSpan.FromSeconds(FadeOutTime)));
            fadeOutAnimation.Completed += new EventHandler(fadeOutAnimation_Completed);
            fadeOutAnimation.Freeze(); // <-- Prevent editing during fadeout

            // Start the animation that changes the Opacity of the adorener
            adorner.BeginAnimation(FrameworkElement.OpacityProperty, fadeOutAnimation);

            adornerShowState = AdornerShowState.FadingOut;
        }

        /// <summary>
        /// Shows or hides the adorner.
        /// Set to 'true' to show the adorner or 'false' to hide the adorner.
        /// </summary>
        public bool IsAdornerVisible
        {
            get
            {
                return (bool)GetValue(IsAdornerVisibleProperty);
            }
            set
            {
                SetValue(IsAdornerVisibleProperty, value);
            }
        }

        /// <summary>
        /// Used in XAML to define the UI content of the adorner.
        /// </summary>
        public DropDownAdornerControl AdornerContent
        {
            get
            {
                return (DropDownAdornerControl)GetValue(AdornerContentProperty);
            }
            set
            {
                SetValue(AdornerContentProperty, value);
            }
        }

        /// <summary>
        /// Specifies the horizontal placement of the adorner relative to the adorned control.
        /// </summary>
        public AdornerPlacement HorizontalAdornerPlacement
        {
            get
            {
                return (AdornerPlacement)GetValue(HorizontalAdornerPlacementProperty);
            }
            set
            {
                SetValue(HorizontalAdornerPlacementProperty, value);
            }
        }

        /// <summary>
        /// Specifies the vertical placement of the adorner relative to the adorned control.
        /// </summary>
        public AdornerPlacement VerticalAdornerPlacement
        {
            get
            {
                return (AdornerPlacement)GetValue(VerticalAdornerPlacementProperty);
            }
            set
            {
                SetValue(VerticalAdornerPlacementProperty, value);
            }
        }

        /// <summary>
        /// Specifies the time (in seconds) it takes to fade in the adorner.
        /// </summary>
        public double FadeInTime
        {
            get
            {
                return (double)GetValue(FadeInTimeProperty);
            }
            set
            {
                SetValue(FadeInTimeProperty, value);
            }
        }

        /// <summary>
        /// Specifies the time (in seconds) it takes to fade out the adorner.
        /// </summary>
        public double FadeOutTime
        {
            get
            {
                return (double) GetValue(FadeOutTimeProperty);
            }
            set
            {
                SetValue(FadeOutTimeProperty, value);
            }
        }

        /// <summary>
        /// Specifies the data that is held within the adorner.
        /// </summary>
        public string Data
        {
            get
            {
                return (string)GetValue(DataProperty);
            }
            set
            {
                SetValue(DataProperty, value);
            }
        }

        /// <summary>
        /// Event raised when the adorner is shown.
        /// </summary>
        public event AdornerEventHandler AdornerShown
        {
            add { AddHandler(AdornerShownEvent, value); }
            remove { RemoveHandler(AdornerShownEvent, value); }
        }

        /// <summary>
        /// Event raised when the adorner is hidden.
        /// </summary>
        public event AdornerEventHandler AdornerHidden
        {
            add { AddHandler(AdornerHiddenEvent, value); }
            remove { RemoveHandler(AdornerHiddenEvent, value); }
        }

        #region Private Data Members

        /// <summary>
        /// Specifies the current show/hide state of the adorner.
        /// </summary>
        private enum AdornerShowState
        {
            Visible,
            Hidden,
            FadingIn,
            FadingOut,
        }

        /// <summary>
        /// Specifies the current show/hide state of the adorner.
        /// </summary>
        private AdornerShowState adornerShowState = AdornerShowState.Hidden;

        /// <summary>
        /// Caches the adorner layer.
        /// </summary>
        private AdornerLayer adornerLayer = null;

        /// <summary>
        /// The actual adorner create to contain our 'adorner UI content'.
        /// </summary>
        private DropDownAdorner adorner = null;
        
        #endregion

        #region Private/Internal Functions

        /// <summary>
        /// Event raised when the DataContext of the adorned control changes.
        /// </summary>
        private void AdornedControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateAdornerDataContext();
        }

        /// <summary>
        /// Update the DataContext of the adorner from the adorned control.
        /// </summary>
        private void UpdateAdornerDataContext()
        {
            if (this.AdornerContent != null)
            {
                this.AdornerContent.DataContext = this.DataContext;
            }
        }

        /// <summary>
        /// Event raised when the value of IsAdornerVisible has changed.
        /// </summary>
        private static void IsAdornerVisible_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AdornedControl c = (AdornedControl)o;
            if (c.AdornerContent == null)
            {
                return;
            }

            c.ShowOrHideAdornerInternal();
        }

        /// <summary>
        /// Event raised when the value of AdornerContent has changed.
        /// </summary>
        private static void AdornerContent_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AdornedControl c = (AdornedControl)o;
            c.ShowOrHideAdornerInternal();

            DropDownAdornerControl oldAdornerContent = (DropDownAdornerControl)e.OldValue;
            if (oldAdornerContent != null)
            {
                oldAdornerContent.OkButtonClick -= new RoutedEventHandler(c.adornerContent_OkButtonClicked);
                oldAdornerContent.CancelButtonClick -= new RoutedEventHandler(c.adornerContent_CancelButtonClicked);
            }

            DropDownAdornerControl newAdornerContent = (DropDownAdornerControl)e.NewValue;
            if (newAdornerContent != null)
            {
                newAdornerContent.OkButtonClick += new RoutedEventHandler(c.adornerContent_OkButtonClicked);
                newAdornerContent.CancelButtonClick += new RoutedEventHandler(c.adornerContent_CancelButtonClicked);
            }
        }

        /// <summary>
        /// Event raised when the ok button clicked in the adorner (save value to data).
        /// </summary>
        private void adornerContent_OkButtonClicked(object sender, EventArgs e)
        {
            if (IsAdornerVisible)
            {
                //Save the edited data
                this.Data = AdornerContent.Value;

                FadeOutAdorner();
            }
        }

        /// <summary>
        /// Event raised when the cancel button is clicked in the adorner.
        /// </summary>
        private void adornerContent_CancelButtonClicked(object sender, EventArgs e)
        {
            if(IsAdornerVisible)
            {
                FadeOutAdorner();
            }
        }

        /// <summary>
        /// Internal method to show or hide the adorner based on the value of IsAdornerVisible.
        /// </summary>
        private void ShowOrHideAdornerInternal()
        {
            if (IsAdornerVisible)
            {
                ShowAdornerInternal();
            }
            else
            {
                HideAdornerInternal();
            }
        }

        /// <summary>
        /// Internal method to show the adorner.
        /// </summary>
        private void ShowAdornerInternal()
        {
            if (this.adorner != null)
            {
                return;
            }

            AddAdorner();

            RaiseEvent(new AdornerEventArgs(AdornerShownEvent, this, this.adorner.Child));
        }

        /// <summary>
        /// Internal method to hide the adorner.
        /// </summary>
        private void HideAdornerInternal()
        {
            if (this.adornerLayer == null || this.adorner == null)
            {
                return;
            }

            RaiseEvent(new AdornerEventArgs(AdornerHiddenEvent, this, this.adorner.Child));

            RemoveAdorner();
        }

        /// <summary>
        /// Called to build the visual tree.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ShowOrHideAdornerInternal();
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if(!IsAdornerVisible)
            {   
                // Set the adorner value
                AdornerContent.Value = this.Data;
                FadeInAdorner();
            }
            else
            {
                FadeOutAdorner();
            }
        }

        /// <summary>
        /// Event raised when the fade in animation has completed.
        /// </summary>
        private void fadeInAnimation_Completed(object sender, EventArgs e)
        {
            if (adornerShowState == AdornerShowState.FadingIn)
            {
                adornerShowState = AdornerShowState.Visible;
                //Focus the adorner
                adorner.Focused();
            }
        }

        /// <summary>
        /// Event raised when the fade-out animation has completed.
        /// </summary>
        private void fadeOutAnimation_Completed(object sender, EventArgs e)
        {
            if (adornerShowState == AdornerShowState.FadingOut)
            {
                this.HideAdorner();
            }
        }

        /// <summary>
        /// Instance the adorner and add it to the adorner layer.
        /// </summary>
        private void AddAdorner()
        {
            if (this.AdornerContent != null)
            {
                if (this.adornerLayer == null)
                {
                    this.adornerLayer = AdornerLayer.GetAdornerLayer(this);
                }

                if (this.adornerLayer != null)
                {
                    FrameworkElement adornedControl = this; // The control to be adorned defaults to 'this'.

                    this.adorner = new DropDownAdorner(this.AdornerContent, adornedControl, 
                                                               this.HorizontalAdornerPlacement, this.VerticalAdornerPlacement);
                    this.adornerLayer.Add(this.adorner);

					//
					// Update the layout of the adorner layout so that clients that depend
					// on the 'AdornerShown' event can use the visual tree of the adorner.
					//
					this.adornerLayer.UpdateLayout();

                    UpdateAdornerDataContext();
                }
            }
        }

        /// <summary>
        /// Remove the adorner from the adorner layer and let it be garbage collected.
        /// </summary>
        private void RemoveAdorner()
        {

            if (this.adornerLayer != null && this.adorner != null)
            {
                this.adornerLayer.Remove(this.adorner);
                this.adorner.DisconnectChild();
            }

            this.adorner = null;
            this.adornerLayer = null;

            //
            // Ensure that the state of the adorned control reflects that
            // the the adorner is no longer.
            //
            this.IsAdornerVisible = false;
            this.adornerShowState = AdornerShowState.Hidden;
		}

        #endregion
    }
}
