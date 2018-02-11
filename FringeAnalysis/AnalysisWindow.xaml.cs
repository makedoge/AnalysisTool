#region Using Region

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
//using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.ComponentModel;

#endregion

namespace FringeAnalysis.Controls
{
    /// <summary>
    /// Interaction logic for AnalysisWindow.xaml
    /// </summary>
    public partial class AnalysisWindow : System.Windows.Controls.UserControl
    {
        #region Public Static Varibales

        public static int zindex = 0;

        #endregion

        #region Public Varibales
        ParameterControl gParameterControl;
        #endregion

        #region Dependency Properties

        /// <summary>
        /// The visible collection of attached children which are not minimized
        /// </summary> 
        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register(
            "Children",
            typeof(ObservableCollection<AnalysisChild>), typeof(AnalysisWindow),
            new PropertyMetadata(new ObservableCollection<AnalysisChild>()));
        public ObservableCollection<AnalysisChild> Children
        {
            get { return (ObservableCollection<AnalysisChild>)GetValue(ChildrenProperty); }
            set { SetValue(ChildrenProperty, value); }
        }
                
        /// <summary>
        /// The visible collection of minmized children
        /// </summary> 
        public static readonly DependencyProperty MinChildrenProperty = DependencyProperty.Register(
            "MinChildren",
            typeof(ObservableCollection<AnalysisChild>), typeof(AnalysisWindow),
            new PropertyMetadata(new ObservableCollection<AnalysisChild>()));
        public ObservableCollection<AnalysisChild> MinChildren
        {
            get { return (ObservableCollection<AnalysisChild>)GetValue(MinChildrenProperty); }
            set { SetValue(MinChildrenProperty, value); }
        }

        /// <summary>
        /// The collection of child which are currently detached
        /// </summary>
        public static readonly DependencyProperty DetachedChildrenProperty = DependencyProperty.Register(
            "DetachedChildren",
            typeof(List<AnalysisChild>), typeof(AnalysisWindow),
            new PropertyMetadata(new List<AnalysisChild>()));
        public List<AnalysisChild> DetachedChildren
        {
            get { return (List<AnalysisChild>)GetValue(DetachedChildrenProperty); }
            set { SetValue(DetachedChildrenProperty, value); }
        }

        /// <summary>
        /// The collection of children which are currently attached 
        /// </summary>
        public static readonly DependencyProperty AttachedChildrenProperty = DependencyProperty.Register(
            "AttachedChildren",
            typeof(ObservableCollection<AnalysisChild>), typeof(AnalysisWindow),
            new PropertyMetadata(new ObservableCollection<AnalysisChild>()));
        public ObservableCollection<AnalysisChild> AttachedChildren
        {
            get { return (ObservableCollection<AnalysisChild>)GetValue(AttachedChildrenProperty); }
            set { SetValue(AttachedChildrenProperty, value); }
        }    

        #endregion

        #region Private Variables

        /// <summary>
        /// The currently selected child
        /// </summary>
        private AnalysisChild mSelectedChild = null;
        public AnalysisChild SelectedChild
        {
            get { return mSelectedChild; }
            set { mSelectedChild = value; }
        }

        ImageAnalysis _imageanalysis = new ImageAnalysis();
        private double gwidth, gheight;

        #endregion

        #region Commands

        /// <summary>
        /// A command to Cascade the attached windows 
        /// </summary>
        public readonly static RoutedUICommand Cascade = new RoutedUICommand("Cascade the windows.",
            "Cascade", typeof(AnalysisWindow));

        /// <summary>
        /// A command to Tile the attached windows
        /// </summary>
        public readonly static RoutedUICommand Tile = new RoutedUICommand("Tile the windows.",
            "Tile", typeof(AnalysisWindow));

        /// <summary>
        /// A command to horizontally tile attached windows
        /// </summary>
        public readonly static RoutedUICommand TileHorizontally = new RoutedUICommand("Tile the windows horizontally.",
            "TileHorizontally", typeof(AnalysisWindow));
        
        /// <summary>
        /// A command to vertically tile attached windows
        /// </summary>
        public readonly static RoutedUICommand TileVertically = new RoutedUICommand("Tile the windows vertically.",
            "TileVertically", typeof(AnalysisWindow));

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for an AnalysisWindow
        /// </summary>
        public AnalysisWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            this.Children.CollectionChanged += new NotifyCollectionChangedEventHandler(Children_CollectionChanged);
            this.MinChildren.CollectionChanged += new NotifyCollectionChangedEventHandler(MinChildren_CollectionChanged);
        }
                
        #endregion

        #region Events
        /// <summary>
        /// Handle a change in the MinChildren collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MinChildren_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (AnalysisChild _child in e.NewItems)
                {
                    AttachedChildren.Add(_child);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (AnalysisChild _child in e.OldItems)
                {
                    AttachedChildren.Remove(_child);
                }
            }
        }
        
        /// <summary>
        /// Handle a change in the Children collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (AnalysisChild _child in e.NewItems)
                {
                    if (_child.Width > this.AnalysisGrid.ActualWidth)
                    {
                        _child.Width = this.AnalysisGrid.ActualWidth;
                        Canvas.SetLeft(_child, 0);
                    }
                    if (_child.Height > this.AnalysisGrid.ActualHeight)
                    {
                        _child.Height = this.AnalysisGrid.ActualHeight;
                        Canvas.SetTop(_child, 0);
                    }
                    // if the child doesn't exist as an attached or detached window the init its zindex, parent and set it unselected
                    if (!this.AttachedChildren.Contains(_child) && !this.DetachedChildren.Contains(_child))
                    {
                        _child.MwiParent = this; // a chld was added, make sure its parent is set to this control
                        _child.IsSelected = false;
                        Canvas.SetZIndex(_child, zindex++);
                    }
                    AttachedChildren.Add(_child);                    
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (AnalysisChild _child in e.OldItems)
                {
                    AttachedChildren.Remove(_child);
                }
            }
        }

        #endregion

        #region Public Methods
        #region Arrangement Methods
        /// <summary>
        /// Vertically Tile the children which are attached and not minimized
        /// </summary>
        public void VerticallyTileChildren()
        {
            int col = 0;
            double maxheight = (this.ActualHeight - 3);
            double maxwidth = (this.ActualWidth - 3) / Children.Count;

            foreach (AnalysisChild _child in Children)
            {
                if (maxwidth < _child.MinSize.Width)
                    maxwidth = _child.MinSize.Width;
                if (maxheight < _child.MinSize.Height)
                    maxheight = _child.MinSize.Height;

                _child.Width = maxwidth;
                _child.Height = maxheight;

                Canvas.SetLeft(_child, col * maxwidth);
                Canvas.SetTop(_child, 0);

                col++;

                if (((col + 1) * maxwidth) > (this.ActualWidth - 3)) 
                    col = 0;
            }
        }

        /// <summary>
        /// Horizontally Tile the children which are attached and not minimized
        /// </summary>
        public void HorizontallyTileChildren()
        {
            int row = 0;
            double maxheight = (this.ActualHeight - 3) / Children.Count;
            double maxwidth = (this.ActualWidth - 3);

            foreach (AnalysisChild _child in Children)
            {
                if (maxwidth < _child.MinSize.Width)
                    maxwidth = _child.MinSize.Width;
                if (maxheight < _child.MinSize.Height)
                    maxheight = _child.MinSize.Height;

                _child.Width = maxwidth;
                _child.Height = maxheight;

                Canvas.SetLeft(_child, 0);
                Canvas.SetTop(_child, row * maxheight);

                row++;

                if (((row + 1) * maxheight) > (this.ActualHeight - 3))
                    row = 0;
            }
        }

        /// <summary>
        /// Tile the children which are attached and not minimized
        /// </summary>
        public void TileChildren()
        {
            int row = -1;
            int column = 0;
            double max = Math.Ceiling(Math.Sqrt(this.Children.Count));
            double remainder = (max * max) - Children.Count;
            double maxwidth = (this.ActualWidth - 3) / max;
            double maxheight = 0;
            foreach (AnalysisChild _child in Children)
            {
                if (remainder > 0)
                {
                    
                    if (row < (max - 1 - (Math.Ceiling(remainder / max))))
                        row++;
                    else
                    {
                        row = 0;
                        column++;
                        remainder--;                        
                    }

                    maxheight = (this.ActualHeight - 3) / (max - (Math.Ceiling(remainder / max)));

                }
                else
                {
                    maxheight = (this.ActualHeight - 3) / max;

                    if (row < (max - 1))
                        row++;
                    else
                    {
                        row = 0;
                        column++;
                    }
                }

                _child.Width = maxwidth;
                _child.Height = maxheight;
                Canvas.SetLeft(_child, column * maxwidth);
                Canvas.SetTop(_child, row * maxheight);
            }
        }

        /// <summary>
        /// Cascade the children which are attached and not minimized
        /// </summary>
        public void CascadeChildren()
        {
            // min height and min width based on number of children and size of MdiWindow
            double minheight = 100d;
            double minwidth = 100d;
            double maxheight = (this.ActualHeight - 3) - (Children.Count - 1) * 28d;
            double maxwidth = (this.ActualWidth - 3) - (Children.Count - 1) * 28d;
            int row = -1;
            int column = -1;
            foreach (AnalysisChild _child in Children)
            {
                minheight = 100; // _child.MinSize.Height; // 100;
                minwidth = _child.MinSize.Width;

                if (maxheight < minheight)
                    maxheight = minheight;
                if (maxwidth < minwidth)
                    maxwidth = minwidth;
                
                row++;
                column++;

                if ((((column * _child.MinSize.Height) + /*_child.ActualHeight*/maxheight) > (this.ActualHeight - 3)) ||
                    (((row * _child.MinSize.Height) + /*_child.ActualWidth*/maxwidth) > (this.ActualWidth - 3)))
                {
                    column = 0;
                    row = 0;
                }
                
                _child.Width = maxwidth;
                _child.Height = maxheight;                

                _child.IsSelected = true;                

                Canvas.SetLeft(_child, column * _child.MinSize.Height);
                Canvas.SetTop(_child, row * _child.MinSize.Height);                
            }
        }
        #endregion

        /// <summary>
        /// Creates a new MwiChild and places it in the Children collection
        /// </summary>
        public void CreateNewMwiChild()
        {
            AnalysisChild child = new AnalysisChild();
            
            double offset = 14;
            double limit = Math.Floor(((this.ActualWidth - child.MinSize.Width) / offset) - 2);
            if (Math.Floor(((this.ActualHeight - child.MinSize.Height) / offset) - 2) < limit)
                limit = Math.Floor((this.ActualHeight / offset) - 2);
                        
            child.Width = this.ActualWidth - (((this.Children.Count % limit) + 1) * offset);
            child.Height = this.ActualHeight - (((this.Children.Count % limit) + 1) * offset);
            child.Background = new SolidColorBrush(Colors.Blue);
            child.BorderBrush = new SolidColorBrush(Colors.Black);
            
            child.Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/app.ico"));
            child.Title = String.Format("My{0}Computer Test 1234567890", zindex); 
            
            Canvas.SetLeft(child, (((this.Children.Count % limit)) * offset));
            Canvas.SetTop(child, (((this.Children.Count % limit)) * offset));
            
            Children.Add(child);
        }
        
        /// <summary>
        /// Creates a new MwiChild and places it in the Children collection
        /// </summary>
        public void OpenImageAnalysisChild(string title)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                        "Portable Network Graphic (*.png)|*.png";
            BitmapSource bitmapsrc;
            //Bitmap bitmap;
            if (op.ShowDialog() == DialogResult.OK)
            {
                bitmapsrc = new BitmapImage(new Uri(op.FileName));      // read bmp file and convert to gray image.
                if (bitmapsrc.Format != PixelFormats.Gray8)
                    bitmapsrc = new FormatConvertedBitmap(bitmapsrc, PixelFormats.Gray8, null, 0);

                AnalysisChild child = null;
                // find a imagewindow
                foreach (AnalysisChild cwin in Children)
                {
                    if (cwin.Title == "ImageWindow")
                    {
                        child = cwin;
                        break;
                    }
                }

                if (child == null)
                {
                    child = new AnalysisChild();
                    double offset = 14;
                    double limit = Math.Floor(((this.ActualWidth - child.MinSize.Width) / offset) - 2);
                    if (Math.Floor(((this.ActualHeight - child.MinSize.Height) / offset) - 2) < limit)
                        limit = Math.Floor((this.ActualHeight / offset) - 2);

                    child.Width = this.ActualWidth - (((this.Children.Count % limit) + 1) * offset);
                    child.Height = this.ActualHeight - (((this.Children.Count % limit) + 1) * offset);
                    child.Background = new SolidColorBrush(Colors.Gray);
                    child.BorderBrush = new SolidColorBrush(Colors.Black);

                    //child.Icon = new BitmapImage(new Uri(op.FileName));
                    child.Icon = bitmapsrc;
                    child.Title = title;
                    System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                    img.Name = @"ImageViewer";
                    img.Source = bitmapsrc;
                    child.AnalysisGrid.Children.Add(img);
                    Canvas.SetLeft(child, (((this.Children.Count % limit)) * offset));
                    Canvas.SetTop(child, (((this.Children.Count % limit)) * offset));

                    Children.Add(child);
                }
                else
                {
                    var img = new System.Windows.Controls.Image();
                    foreach(var element in child.AnalysisGrid.Children)
                    {
                        if(element.GetType() == typeof(System.Windows.Controls.Image))
                        {
                            img = (System.Windows.Controls.Image)element;
                        }
                    }
                    img.Source = bitmapsrc;
                }
                if (_imageanalysis == null)
                    _imageanalysis = new ImageAnalysis();
                _imageanalysis.FringesImage = bitmapsrc;
                
            }
        }

        /// <summary>
        /// Creates a new MwiChild and places it in the Children collection
        /// </summary>
        /// 
        public void getWindow(string name, ref AnalysisChild child)
        {
            child = null;
            // find a imagewindow
            foreach (AnalysisChild cwin in Children)
            {
                if (cwin.Name == name)
                {
                    child = cwin;
                    return;
                }
            }

            if (child == null)
            {
                child = new AnalysisChild();

                double offset = 14;
                Loaded += delegate
                {
                    this.gwidth = this.ActualWidth;
                    this.gheight = this.ActualHeight;
                };

                if (this.gwidth == 0 && this.gheight == 0)
                {
                    this.gheight = 1024;
                    this.gwidth = 1284;
                }
                double limit = Math.Floor(((gwidth - child.MinSize.Width) / offset) - 2);
                if (Math.Floor(((gheight - child.MinSize.Height) / offset) - 2) < limit)
                    limit = Math.Floor((gheight / offset) - 2);

                child.Width = gwidth - (((this.Children.Count % limit) + 1) * offset);
                child.Height = gheight - (((this.Children.Count % limit) + 1) * offset);

                child.Background = new SolidColorBrush(Colors.Gray);
                child.BorderBrush = new SolidColorBrush(Colors.Black);

                child.Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/app.ico"));
                child.Name = name;
                Canvas.SetLeft(child, (((this.Children.Count % limit)) * offset));
                Canvas.SetTop(child, (((this.Children.Count % limit)) * offset));
                Children.Add(child);
            }
        }

        public void ParameterChild()
        {
            AnalysisChild child = null;
            getWindow("ParameterWindow", ref child);
            gParameterControl = new ParameterControl();
            child.Title = @"Parameter Window";
            child.Name = @"ParameterWindow";
            child.AnalysisGrid.Children.Add(gParameterControl);
        }

        public void LogChild()
        {
            AnalysisChild child = null;
            getWindow("LogWindow", ref child);
            System.Windows.Controls.TextBox logTextBox = new System.Windows.Controls.TextBox();
            child.Title = @"Log Window";
            child.Name = @"LogWindow";
            child.AnalysisGrid.Children.Add(logTextBox);
        }
        
        public void CutImageAnalysisChild()
        {
            if (_imageanalysis.FringesImage == null)
                return;
            int ledge=0, redge=0, tedge=0, bedge=0;
            AnalysisChild child = null;
            var img = new System.Windows.Controls.Image();
            foreach (AnalysisChild cwin in Children)
            {
                if (cwin.Title == "RemoveEdge")
                {
                    child = cwin;
                    break;
                }
            }
            if (child == null)
            {
                child = new AnalysisChild();
                getWindow("RemoveEdge", ref child);
                //System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                img.Name = @"ImageViewer";
                child.AnalysisGrid.Children.Add(img);
            }

            BitmapSource processImage;
            processImage = _imageanalysis.CutEdge(_imageanalysis.FringesImage, 0, ref ledge, ref redge, ref tedge, ref bedge);

            img = new System.Windows.Controls.Image();
            foreach (var element in child.AnalysisGrid.Children)
            {
                if (element.GetType() == typeof(System.Windows.Controls.Image))
                {
                    img = (System.Windows.Controls.Image)element;
                }
            }
            img.Source = processImage;
        }
        
        public void ImageFFTAnalysisChild()
        {

        }

        public void ImageEdgeAnalysisChild()
        {
            if (_imageanalysis.FringesImage == null)
                return;
            AnalysisChild child = null;
            var img = new System.Windows.Controls.Image();
            foreach (AnalysisChild cwin in Children)
            {
                if (cwin.Title == "ImageEdgeWindow")
                {
                    child = cwin;
                    break;
                }
            }
            if (child == null)
            {
                child = new AnalysisChild();
                getWindow("ImageEdgeWindow", ref child);
                //System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                img.Name = @"ImageViewer";
                child.AnalysisGrid.Children.Add(img);
            }

            BitmapSource processImage;
            processImage = _imageanalysis.FindEdge();

            img = new System.Windows.Controls.Image();
            foreach (var element in child.AnalysisGrid.Children)
            {
                if (element.GetType() == typeof(System.Windows.Controls.Image))
                {
                    img = (System.Windows.Controls.Image)element;
                }
            }
            img.Source = processImage;
        }

        public void CircleToSquareAnalysisChild()
        {
            if (_imageanalysis.FringesImage == null)
                return;
            AnalysisChild child = null;
            var img = new System.Windows.Controls.Image();
            foreach (AnalysisChild cwin in Children)
            {
                if (cwin.Title == "SquareWindow")
                {
                    child = cwin;
                    break;
                }
            }
            if (child == null)
            {
                child = new AnalysisChild();
                getWindow("SquareWindow", ref child);
                //System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                img.Name = @"ImageViewer";
                child.AnalysisGrid.Children.Add(img);
            }

            BitmapSource processImage;
            processImage = _imageanalysis.CycleToSquare();
            //processImage = _imageanalysis.CutEdge(_imageanalysis.FringesImage, 0, ref ledge, ref redge, ref tedge, ref bedge);

            img = new System.Windows.Controls.Image();
            foreach (var element in child.AnalysisGrid.Children)
            {
                if (element.GetType() == typeof(System.Windows.Controls.Image))
                {
                    img = (System.Windows.Controls.Image)element;
                }
            }
            img.Source = processImage;
        }

        public void SquareToCircleAnalysisChild()
        {
            if (_imageanalysis.FringesImage == null)
                return;
            AnalysisChild child = null;
            var img = new System.Windows.Controls.Image();
            foreach (AnalysisChild cwin in Children)
            {
                if (cwin.Title == "SquareWindow")
                {
                    child = cwin;
                    break;
                }
            }
            if (child == null)
            {
                child = new AnalysisChild();
                getWindow("SquareWindow", ref child);
                //System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                img.Name = @"ImageViewer";
                child.AnalysisGrid.Children.Add(img);
            }

            BitmapSource processImage;
            processImage = _imageanalysis.SquareToCycle();
            //processImage = _imageanalysis.CutEdge(_imageanalysis.FringesImage, 0, ref ledge, ref redge, ref tedge, ref bedge);

            img = new System.Windows.Controls.Image();
            foreach (var element in child.AnalysisGrid.Children)
            {
                if (element.GetType() == typeof(System.Windows.Controls.Image))
                {
                    img = (System.Windows.Controls.Image)element;
                }
            }
            img.Source = processImage;
        }

        public void ImageProcessAnalysisChild()
        {
        }

        /// <summary>
        /// Minimizes a child in the MwiWindow
        /// </summary>
        /// <param name="child"></param>
        /// <param name="reverse"></param>
        public void MinimizeChild(AnalysisChild child, bool reverse)
        {
            if (!reverse)
            {
                if (Children.Remove(child))
                {
                    MinChildren.Add(child);
                }
            }
            else
            {
                if (MinChildren.Remove(child))
                {
                    Children.Add(child);
                }
            }
        }

        /// <summary>
        /// Detaches or attaches a child to the MwiWindow
        /// </summary>
        /// <param name="child"></param>
        /// <param name="reverse"></param>
        public void DetachChild(AnalysisChild child, bool reverse)
        {
            if (!reverse)
            {
                if (Children.Remove(child))
                {
                    DetachedChildren.Add(child);                    
                }
            }
            else
            {
                if (DetachedChildren.Remove(child))
                {
                    Children.Add(child);                    
                }
            }
        }

        /// <summary>
        /// Promotes the child to top-most in the MwiWindow
        /// </summary>
        /// <param name="child"></param>
        public void PromoteChildToFront(AnalysisChild child)
        {
            int zindex = Canvas.GetZIndex(child);
            foreach (AnalysisChild _child in AttachedChildren) //Children)
            {
                _child.IsSelected = _child.Equals(child);
                
                int _zindex = Canvas.GetZIndex(_child);
                Canvas.SetZIndex(_child, _child.Equals(child) ? AnalysisWindow.zindex - 1 : _zindex < zindex ? _zindex : _zindex - 1);
            }
            if (child.IsSelected && child.IsMinimized)
                child.Minimize(true);
            this.SelectedChild = child;
        }

        private Bitmap ImageSourceToBitmap(BitmapSource bitmapimage)
        {
            Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapimage));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
                return bitmap;
            }
        }

        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
        
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ParameterChild();
            TileChildren();
        }
   
    }
}