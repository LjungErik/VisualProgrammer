using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using VisualProgrammer.Controls;
using VisualProgrammer.Data;
using VisualProgrammer.Utilities.Processing.FileHelpers;
using VisualProgrammer.ViewModels;

namespace VisualProgrammer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VisualProject visualProject;

        private bool isUnsaved = false;

        private IFileHandler fileHandler;

        public MainWindow()
        {
            InitializeComponent();

            DragHandler.SetRoot(this);
            fileHandler = new XMLFileHandler();
        }

        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (designContent.Visibility == Visibility.Visible);
        }

        private void NewAction_Command(object sender, ExecutedRoutedEventArgs e)
        {
            if (!TryToSaveCurrent())
                return;

            visualProject = GetDefaultProject();

            SetProject(visualProject);           
        }

        private void OpenAction_Command(object sender, ExecutedRoutedEventArgs e)
        {
            if (!TryToSaveCurrent())
                return;

            visualProject = OpenProject();

            if(visualProject == null)
                return;

            SetProject(visualProject);
        }

        private void SaveAction_Command(object sender, ExecutedRoutedEventArgs e)
        {
            if(!SaveCurrent())
                return;

            projectNameLabel.Content = visualProject.ProjectName;
        }

        private void SaveAsAction_Command(object sender, ExecutedRoutedEventArgs e)
        {
            if (!SaveCurrent(true))
                return;

            projectNameLabel.Content = visualProject.ProjectName;
        }

        private void BuildAction_Command(object sender, ExecutedRoutedEventArgs e)
        {
            if (designerControl.ViewModel.Designer.StartNode == null)
                return;

            CompilerStatusWindow compileStatusWindow = new CompilerStatusWindow(designerControl.ViewModel.Designer.StartNode);
            compileStatusWindow.Owner = Window.GetWindow(this);
            compileStatusWindow.ShowDialog();
        }

        private string GetProjectName(string filename)
        {
            return System.IO.Path.GetFileNameWithoutExtension(filename);
        }

        private void OnDesignerChanged(object sender, EventArgs e)
        {
            if (isUnsaved)
                return;

            isUnsaved = true;
            projectNameLabel.Content = visualProject.ProjectName + "*";
        }

        #region Private Help methods

        private bool TryToSaveCurrent()
        {
            if (visualProject == null)
                return true;

            switch (ConfirmSave())
            {
                case MessageBoxResult.Yes:
                    return SaveProject(visualProject);
                case MessageBoxResult.No:
                    return true;
            }
            return false;
        }

        private MessageBoxResult ConfirmSave()
        {
            var result = MessageBox.Show("You have uncommited changes. Do you want to save?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            return result;
        }

        private bool SaveCurrent()
        {
            if (visualProject == null)
                return true;

            return SaveProject(visualProject);
        }

        private bool SaveProject(VisualProject project, bool saveAs = false)
        {
            if (saveAs || String.IsNullOrEmpty(project.ProjectDirectory))
                GetSavingLocation(project);

            var fullPath = project.ProjectDirectory;

            project.Data = designerControl.ViewModel.GetData();

            return SaveAt(project, fullPath);
        }

        private void GetSavingLocation(VisualProject project)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.FileName = project.ProjectName;
            saveDialog.DefaultExt = ".xml";
            saveDialog.Filter = "Project documents (.xml) | *.xml";

            if (saveDialog.ShowDialog() == true)
            {
                project.ProjectDirectory = saveDialog.FileName;
                project.ProjectName = GetProjectName(saveDialog.FileName);
            }
        }

        private bool SaveAt(VisualProject project, string fullPath)
        {
            if (String.IsNullOrEmpty(fullPath))
                return false;

            try
            {
                fileHandler.Save<VisualProject>(fullPath, project);
            }
            catch
            {
                //Message box to display error
                MessageBox.Show("An error was encountered while trying to save the project.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private VisualProject GetDefaultProject()
        {
            return new VisualProject()
            {
                Data = new DesignerData()
                {
                    Nodes = new List<Node>(),
                    Connections = new List<Connection>()
                },
                ProjectName = "NewProject"
            };
        }

        private void SetProject(VisualProject project)
        {
            projectNameLabel.Content = project.ProjectName;

            var designerViewModel = new DesignerControlViewModel(project.Data);
            designerControl.DataContext = designerViewModel;
            designContent.Visibility = Visibility.Visible;
        }

        private VisualProject OpenProject()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.DefaultExt = ".xml";
            openDialog.Filter = "Project documents (.xml) | *.xml";

            VisualProject project = null;

            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    project = fileHandler.Load<VisualProject>(openDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Error, the selected file could not be opened. Invalid file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    project = null;
                }
            }
            return project;
        }

        #endregion
    }
}
