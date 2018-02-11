#region Using Region
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FringeAnalysis.Controls;
using System.Collections.ObjectModel;
#endregion

namespace AnalysisTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : System.Windows.Window
    {
        #region Constructor

        public MainWindow()
        {
            InitializeComponent();

            CommandBinding cmdBinding = null;
            // Always show the close command and upon execution close the application
            cmdBinding = new CommandBinding(ApplicationCommands.Close);
            cmdBinding.PreviewExecuted += new ExecutedRoutedEventHandler(delegate(object sender, ExecutedRoutedEventArgs e) { this.Close(); e.Handled = true; });
            cmdBinding.CanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }); 
            cmdBinding.PreviewCanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; });
            CommandBindings.Add(cmdBinding);

            // Always show the New command and upon execution create a new window in the MwiWindow control.
            cmdBinding = new CommandBinding(ApplicationCommands.New);
            cmdBinding.PreviewExecuted += new ExecutedRoutedEventHandler(delegate(object sender, ExecutedRoutedEventArgs e) { this.gAnalysisWindow.CreateNewMwiChild(); e.Handled = true; });
            cmdBinding.CanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; });
            cmdBinding.PreviewCanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; });
            CommandBindings.Add(cmdBinding);

            // Only show the Cascade command when there are child windows. Upon execution rearrange the child windows so that they are cascading.
            cmdBinding = new CommandBinding(AnalysisWindow.Cascade);
            cmdBinding.PreviewExecuted += new ExecutedRoutedEventHandler(delegate(object sender, ExecutedRoutedEventArgs e) { this.gAnalysisWindow.CascadeChildren(); e.Handled = true; });
            cmdBinding.CanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { if (this.gAnalysisWindow.AttachedChildren.Count > 0) { e.CanExecute = true; } });
            cmdBinding.PreviewCanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { if (this.gAnalysisWindow.AttachedChildren.Count > 0) { e.CanExecute = true; } }); 
            CommandBindings.Add(cmdBinding);

            // Only show the Tile command when there are child windows. Upon execution rearrange the child windows so that they are tiled.
            cmdBinding = new CommandBinding(AnalysisWindow.Tile);
            cmdBinding.PreviewExecuted += new ExecutedRoutedEventHandler(delegate(object sender, ExecutedRoutedEventArgs e) { this.gAnalysisWindow.TileChildren(); e.Handled = true; });
            cmdBinding.CanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { if (this.gAnalysisWindow.AttachedChildren.Count > 0) { e.CanExecute = true; } });
            cmdBinding.PreviewCanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { if (this.gAnalysisWindow.AttachedChildren.Count > 0) { e.CanExecute = true; } }); 
            CommandBindings.Add(cmdBinding);

            // Only show the Tile Horizontally command when there are child windows. Upon execution rearrange the child windows so that they are tiled horizontally.
            cmdBinding = new CommandBinding(AnalysisWindow.TileHorizontally);
            cmdBinding.PreviewExecuted += new ExecutedRoutedEventHandler(delegate(object sender, ExecutedRoutedEventArgs e) { this.gAnalysisWindow.HorizontallyTileChildren(); e.Handled = true; });
            cmdBinding.CanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { if (this.gAnalysisWindow.AttachedChildren.Count > 0) { e.CanExecute = true; } });
            cmdBinding.PreviewCanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { if (this.gAnalysisWindow.AttachedChildren.Count > 0) { e.CanExecute = true; } }); 
            CommandBindings.Add(cmdBinding);

            // Only show the Tile Vertically command when there are child windows. Upon execution rearrange the child windows so that they are tiled vertically.
            cmdBinding = new CommandBinding(AnalysisWindow.TileVertically);
            cmdBinding.PreviewExecuted += new ExecutedRoutedEventHandler(delegate(object sender, ExecutedRoutedEventArgs e) { this.gAnalysisWindow.VerticallyTileChildren(); e.Handled = true; });
            cmdBinding.CanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { if (this.gAnalysisWindow.AttachedChildren.Count > 0) { e.CanExecute = true; } });
            cmdBinding.PreviewCanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { if (this.gAnalysisWindow.AttachedChildren.Count > 0) { e.CanExecute = true; } });
            CommandBindings.Add(cmdBinding);

            // Load a image.
            cmdBinding = new CommandBinding(ImageAnalysis.LoadImage);
            cmdBinding.PreviewExecuted += new ExecutedRoutedEventHandler(delegate(object sender, ExecutedRoutedEventArgs e) { this.gAnalysisWindow.OpenImageAnalysisChild("ImageWindow"); e.Handled = true; });
            cmdBinding.CanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; });
            cmdBinding.PreviewCanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; });
            CommandBindings.Add(cmdBinding);

            // Cut a image.
            cmdBinding = new CommandBinding(ImageAnalysis.CutImage);
            cmdBinding.PreviewExecuted += new ExecutedRoutedEventHandler(delegate(object sender, ExecutedRoutedEventArgs e) { this.gAnalysisWindow.CutImageAnalysisChild(); e.Handled = true; });
            cmdBinding.CanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; });
            cmdBinding.PreviewCanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; });
            CommandBindings.Add(cmdBinding);

            // Image FFT.
            cmdBinding = new CommandBinding(ImageAnalysis.ImageFFT);
            cmdBinding.PreviewExecuted += new ExecutedRoutedEventHandler(delegate(object sender, ExecutedRoutedEventArgs e) { this.gAnalysisWindow.ImageFFTAnalysisChild(); e.Handled = true; });
            cmdBinding.CanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; });
            cmdBinding.PreviewCanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; });
            CommandBindings.Add(cmdBinding);

            // Image Edge process.
            cmdBinding = new CommandBinding(ImageAnalysis.ImageEdge);
            cmdBinding.PreviewExecuted += new ExecutedRoutedEventHandler(delegate(object sender, ExecutedRoutedEventArgs e) { this.gAnalysisWindow.ImageEdgeAnalysisChild(); e.Handled = true; });
            cmdBinding.CanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; });
            cmdBinding.PreviewCanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; });
            CommandBindings.Add(cmdBinding);

            // Image Edge process.
            cmdBinding = new CommandBinding(ImageAnalysis.CircleToSquare);
            cmdBinding.PreviewExecuted += new ExecutedRoutedEventHandler(delegate (object sender, ExecutedRoutedEventArgs e) { this.gAnalysisWindow.CircleToSquareAnalysisChild(); e.Handled = true; });
            cmdBinding.CanExecute += new CanExecuteRoutedEventHandler(delegate (object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; });
            cmdBinding.PreviewCanExecute += new CanExecuteRoutedEventHandler(delegate (object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; });
            CommandBindings.Add(cmdBinding);

            // Image Edge process.
            cmdBinding = new CommandBinding(ImageAnalysis.SquareToCircle);
            cmdBinding.PreviewExecuted += new ExecutedRoutedEventHandler(delegate (object sender, ExecutedRoutedEventArgs e) { this.gAnalysisWindow.SquareToCircleAnalysisChild(); e.Handled = true; });
            cmdBinding.CanExecute += new CanExecuteRoutedEventHandler(delegate (object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; });
            cmdBinding.PreviewCanExecute += new CanExecuteRoutedEventHandler(delegate (object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; });
            CommandBindings.Add(cmdBinding);

            // Image process.
            cmdBinding = new CommandBinding(ImageAnalysis.ImageProcess);
            cmdBinding.PreviewExecuted += new ExecutedRoutedEventHandler(delegate(object sender, ExecutedRoutedEventArgs e) { this.gAnalysisWindow.ImageProcessAnalysisChild(); e.Handled = true; });
            cmdBinding.CanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; });
            cmdBinding.PreviewCanExecute += new CanExecuteRoutedEventHandler(delegate(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; });
            CommandBindings.Add(cmdBinding);
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}