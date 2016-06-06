using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VisualProgrammer.Enums;
using VisualProgrammer.Factory;
using VisualProgrammer.Factory.MouseActions;

namespace VisualProgrammer.Views.Designer.MouseToolPanel
{
    public class MouseSelectorPanel : Control
    {
        #region Private Data Members

        private Button arrowBtn = null;

        private Button moveBtn = null;

        private Button selectBtn = null;

        private MouseActionFactory factory = null;

        private MouseAction clickedAction;

        #endregion Private Data Members

        #region Dependency Properties

        public static readonly DependencyProperty MouseHandlerProperty =
            DependencyProperty.Register("MouseHandler", typeof(IMouseAction), typeof(MouseSelectorPanel));

        #endregion Dependency Properties

        public MouseSelectorPanel()
        {
            factory = new MouseActionFactory();
            MouseHandler = factory.GetAction(MouseAction.None);
            clickedAction = MouseAction.None;
        }

        public IMouseAction MouseHandler
        {
            get 
            {
                return (IMouseAction)GetValue(MouseHandlerProperty);
            }
            set 
            {
                SetValue(MouseHandlerProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.arrowBtn = (Button)this.Template.FindName("PART_ArrowBtn", this);
            if (this.arrowBtn == null)
            {
                throw new ApplicationException("Failed to locate 'PART_ArrowBtn' in 'MouseSelectorPanel'");
            }

            this.moveBtn = (Button)this.Template.FindName("PART_MoveBtn", this);
            if (this.moveBtn == null)
            {
                throw new ApplicationException("Failed to locate 'PART_MoveBtn' in 'MouseSelectorPanel'");
            }

            this.selectBtn = (Button)this.Template.FindName("PART_SelectBtn", this);
            if (this.selectBtn == null)
            {
                throw new ApplicationException("Failed to locate 'PART_SelectBtn' in 'MouseSelectorPanel'");
            }

            this.arrowBtn.Click += new RoutedEventHandler(ArrowBtn_Clicked);
            this.moveBtn.Click += new RoutedEventHandler(MoveBtn_Clicked);
            this.selectBtn.Click += new RoutedEventHandler(SelectBtn_Clicked);
        }

        #region Private Methods

        static MouseSelectorPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MouseSelectorPanel), new FrameworkPropertyMetadata(typeof(MouseSelectorPanel)));
        }

        private void ArrowBtn_Clicked(object sender, RoutedEventArgs e)
        {
            if (clickedAction != MouseAction.None)
            {
                MouseHandler = factory.GetAction(MouseAction.None);
                clickedAction = MouseAction.None;
            }
        }

        private void MoveBtn_Clicked(object sender, RoutedEventArgs e)
        {
            if (clickedAction != MouseAction.Move)
            {
                MouseHandler = factory.GetAction(MouseAction.Move);
                clickedAction = MouseAction.Move;
            }
        }

        private void SelectBtn_Clicked(object sender, RoutedEventArgs e)
        {
            if (clickedAction != MouseAction.Select)
            {
                MouseHandler = factory.GetAction(MouseAction.Select);
                clickedAction = MouseAction.Select;
            }
        }

        #endregion Private Methods
    }
}
