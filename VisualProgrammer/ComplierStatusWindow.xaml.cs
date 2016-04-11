using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VisualProgrammer.Processing;
using VisualProgrammer.Utilities.Logger;
using VisualProgrammer.ViewModels;
using VisualProgrammer.ViewModels.Designer;

namespace VisualProgrammer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CompilerStatusWindow : Window
    {
        private StartNodeViewModel startNode;

        public CompilerStatusWindow(StartNodeViewModel startNode)
        {
            InitializeComponent();

            //Create needed logger to signal logging of compile events
            SetLogger(new CompileLogger());
            this.startNode = startNode;
        }

        public CompilerStatusViewModel ViewModel
        {
            get
            {
                return (CompilerStatusViewModel)DataContext;
            }
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            //Begin the build process
            Builder.PerformBuild(this.ViewModel.LogOutput.Logger, startNode);
        }

        private void SetLogger(CompileLogger logger)
        {
            this.ViewModel.LogOutput.Logger = logger;
            this.ViewModel.LogProgress.Logger = logger;
        }

        private void OKButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
