using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Emgu.CV;
//using Emgu.Util;
//using Emgu.CV.Structure;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using Microsoft.Win32;
using System.Windows.Media;
using System.Windows.Input;
using System.Threading;

namespace FringeAnalysis.Controls
{
    public class ImageAnalysis
    {
        public BitmapSource FringesImage;

        private Coefficient coef = new Coefficient();
        //public Coefficient Coef { get; set; }
//#pragma warning disable CS0414 // The field 'ImageAnalysis.wavelength' is assigned but its value is never used
        private int wavelength = 652;
        public int WaveLength { get; set; }
//#pragma warning restore CS0414 // The field 'ImageAnalysis.wavelength' is assigned but its value is never used

//#pragma warning disable CS0414 // The field 'ImageAnalysis.incomingAngle' is assigned but its value is never used
        private double incomingAngle = 10;
//#pragma warning restore CS0414 // The field 'ImageAnalysis.incomingAngle' is assigned but its value is never used
//#pragma warning disable CS0414 // The field 'ImageAnalysis.thielta' is assigned but its value is never used
        private double thielta = 0.02;
//#pragma warning restore CS0414 // The field 'ImageAnalysis.thielta' is assigned but its value is never used

//#pragma warning disable CS0169 // The field 'ImageAnalysis.BackGroundImage' is never used
        private byte[] BackGroundImage;
//#pragma warning restore CS0169 // The field 'ImageAnalysis.BackGroundImage' is never used
//#pragma warning disable CS0169 // The field 'ImageAnalysis.Phase2DImage' is never used
        private byte[] Phase2DImage;
//#pragma warning restore CS0169 // The field 'ImageAnalysis.Phase2DImage' is never used

//#pragma warning disable CS0169 // The field 'ImageAnalysis.AnalysisResultImage' is never used
        private byte[] AnalysisResultImage;
//#pragma warning restore CS0169 // The field 'ImageAnalysis.AnalysisResultImage' is never used

        //public Coefficient Coef
        //{
        //    get {
        //        return coef;
        //    }
        //    set {
        //        coef = value;
        //    }
        //}

        #region Commands
        /// <summary>
        /// A command to Load a image file 
        /// </summary>
        public readonly static RoutedUICommand LoadImage = new RoutedUICommand("Load a Image file.",
            "LoadImage", typeof(ImageAnalysis));
        /// <summary>
        /// A command to cut image blank area
        /// </summary>
        public readonly static RoutedUICommand CutImage = new RoutedUICommand("Cut image blank area.",
            "CutImage", typeof(ImageAnalysis));
        /// <summary>
        /// A command to do FFT for a image
        /// </summary>
        public readonly static RoutedUICommand ImageFFT = new RoutedUICommand("Image FFT.",
            "ImageFFT", typeof(ImageAnalysis));
        /// <summary>
        /// A command to image edge
        /// </summary>
        public readonly static RoutedUICommand ImageEdge = new RoutedUICommand("image Edge.",
            "ImageEdge", typeof(ImageAnalysis));

        /// <summary>
        /// A command to process image
        /// </summary>
        public readonly static RoutedUICommand CircleToSquare = new RoutedUICommand("Circle to Square.",
            "ImageProcess", typeof(ImageAnalysis));

        /// <summary>
        /// A command to process image
        /// </summary>
        public readonly static RoutedUICommand SquareToCircle = new RoutedUICommand("Square to Circle.",
            "ImageProcess", typeof(ImageAnalysis));

        /// <summary>
        /// A command to process image
        /// </summary>
        public readonly static RoutedUICommand ImageProcess = new RoutedUICommand("image process.",
            "ImageProcess", typeof(ImageAnalysis));
        #endregion

        public void OpenImage_Click()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                        "Portable Network Graphic (*.png)|*.png";
            BitmapSource bitmapsrc;
            //Bitmap bitmap;
            if (op.ShowDialog() == true)
            {
                bitmapsrc = new BitmapImage(new Uri(op.FileName));      // read bmp file and convert to gray image.
                if (bitmapsrc.Format != PixelFormats.Gray8)
                    bitmapsrc = new FormatConvertedBitmap(bitmapsrc, PixelFormats.Gray8, null, 0);

                //ImageWindow bWin = new ImageWindow();
                //bWin.imgPhoto.Source = bitmapsrc;
                //bWin.Show();

                //lock (g_ImageWindows)
                //{
                //    g_ImageWindows.Add(bWin);
                //}
                //lock (g_historyimagelist)
                //{
                //    g_historyimagelist.Add(bitmapsrc as BitmapImage);
                //    g_currentImageIndex = g_historyimagelist.Count - 1;
                //}
                //g_CurrentWindow = bWin;
                //CutImage.IsEnabled = true;
                //Fft.IsEnabled = true;
                //EdgeMesurement.IsEnabled = true;

                Bitmap bitmap;
                using (MemoryStream outStream = new MemoryStream())
                {
                    BitmapEncoder enc = new BmpBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(bitmapsrc));
                    enc.Save(outStream);
                    bitmap = new System.Drawing.Bitmap(outStream);
                }

                //Image<Gray, float> image = new Image<Gray, float>(bitmap);
                //IntPtr complexImage = CvInvoke.cvCreateImage(image.Size, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_32F, 2);

                //CvInvoke.cvSetZero(complexImage);
                //CvInvoke.cvSetImageCOI(complexImage, 1);
                //CvInvoke.cvCopy(image, complexImage, IntPtr.Zero);
                //CvInvoke.cvSetImageCOI(complexImage, 0);

                //Matrix<float> dft = new Matrix<float>(image.Rows, image.Cols, 2);
                //CvInvoke.cvDFT(complexImage, dft, Emgu.CV.CvEnum.CV_DXT.CV_DXT_FORWARD, 0);

                //Matrix<float> outReal = new Matrix<float>(image.Size);
                //Matrix<float> outIm = new Matrix<float>(image.Size);
                //CvInvoke.cvSplit(dft, outReal, outIm, IntPtr.Zero, IntPtr.Zero);

                //Image<Gray, float> fftImage = new Image<Gray, float>(outReal.Size);
                //CvInvoke.cvCopy(outReal, fftImage, IntPtr.Zero);

                //bWin = new ImageWindow();
                //bWin.Show();
                //bWin.imgPhoto.Source = BitmapToImageSource(image.ToBitmap());

                //bWin = new ImageWindow();
                //bWin.Show();
                //bWin.imgPhoto.Source = BitmapToImageSource(fftImage.Log().ToBitmap());
            }
        }

        public void Redo_Click(object sender, RoutedEventArgs e)
        {
            //if (g_currentImageIndex == NUMBEROFMAXBUFFERIMAGE - 1)
            //    return;

            //if (g_currentImageIndex < NUMBEROFMAXBUFFERIMAGE - 1 && g_currentImageIndex < g_historyimagelist.Count - 1)
            //{
            //    g_currentImageIndex++;
            //    ImageWindow bWin;
            //    bWin = g_ImageWindows.First();
            //    bWin.imgPhoto.Source = g_historyimagelist[g_currentImageIndex];
            //}
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            //if (g_currentImageIndex == 0)
            //    return;

            //if (g_currentImageIndex > 0 && g_currentImageIndex < NUMBEROFMAXBUFFERIMAGE - 1)
            //{
            //    g_currentImageIndex--;
            //    ImageWindow bWin;
            //    bWin = g_ImageWindows.First();
            //    bWin.imgPhoto.Source = g_historyimagelist[g_currentImageIndex];
            //}
        }

        public BitmapSource FindEdge()
        {
            Bitmap bitmap, resizebitmap;
            int width, height;
            byte[] pixels;
            byte[] mask;

            int arrayLength = 0;

            bitmap = ImageSourceToBitmap(FringesImage);
            width = bitmap.Width;
            height = bitmap.Height;

            resizebitmap = new Bitmap(bitmap, width + 4, height + 4);

            BitmapSource newimg = new FormatConvertedBitmap(BitmapToImageSource(resizebitmap), PixelFormats.Gray8, null, 0);

            arrayLength = (width + 4) * (height + 4);
            pixels = new byte[arrayLength];
            mask = new byte[arrayLength];

            pixels = getMetaData(ref resizebitmap);
            for (int i = 0; i < arrayLength; i++)   // mask initial
                mask[i] = 1;

            int fh, fv, fb, ff, h2, hn2, v2, vn2, b2, bn2, f2, fn2;
            for (int n = 0; n < 10; n++)
            {
                for (int i = 2; i < height + 2; i++)
                    for (int j = 2; j < width + 2; j++)
                    {
                        if (mask[i * width + j] == 1)
                        {
                            fh = pixels[i * width + j - 1] + pixels[i * width + j] + pixels[i * width + j + 1];
                            fv = pixels[(i - 1) * width + j] + pixels[i * width + j] + pixels[(i + 1) * width + j];
                            fb = pixels[(i - 1) * width + j + 1] + pixels[i * width + j] + pixels[(i + 1) * width + j - 1];
                            ff = pixels[(i - 1) * width + j - 1] + pixels[i * width + j] + pixels[(i + 1) * width + j + 1];

                            h2 = pixels[(i - 2) * width + j - 1] + pixels[(i - 2) * width + j] + pixels[(i - 2) * width + j + 1];
                            hn2 = pixels[(i + 2) * width + j - 1] + pixels[(i + 2) * width + j] + pixels[(i + 2) * width + j + 1];
                            v2 = pixels[(i - 1) * width + j - 2] + pixels[i * width + j - 2] + pixels[(i + 1) * width + j - 2];
                            vn2 = pixels[(i - 1) * width + j + 2] + pixels[i * width + j + 2] + pixels[(i + 1) * width + j + 2];

                            b2 = pixels[(i - 2) * width + j - 2] + pixels[i * width + j] + pixels[(i + 2) * width + j + 2];
                            bn2 = pixels[(i - 2) * width + j] + pixels[i * width + j] + pixels[(i + 2) * width + j];
                            f2 = pixels[(i - 2) * width + j + 2] + pixels[i * width + j] + pixels[(i + 2) * width + j - 2];
                            fn2 = pixels[(i - 2) * width + j - 2] + pixels[i * width + j] + pixels[(i + 2) * width + j + 2];

                            int k = 0;
                            if (fh > h2 && fh > hn2)
                                k++;
                            if (fv > v2 && fv > vn2)
                                k++;
                            if (fb > b2 && fb > bn2)
                                k++;
                            if (ff > f2 && ff > fn2)
                                k++;

                            if (k < 2)
                                mask[i * width + j] = 0;
                            else
                                mask[i * width + j] = 255;
                        }
                    }
            }

            WriteableBitmap tinted = new WriteableBitmap(newimg);
            tinted.WritePixels(new Int32Rect(0, 0, width+4, height+4), mask, width+4, 0);
            return tinted;

            //BitmapImage bitmapimage;
            //using (MemoryStream memory = new MemoryStream())
            //{
            //    resizebitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
            //    memory.Position = 0;
            //    bitmapimage = new BitmapImage();
            //    bitmapimage.BeginInit();
            //    bitmapimage.StreamSource = memory;
            //    bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
            //    bitmapimage.EndInit();
            //}

            //WriteableBitmap tinted = new WriteableBitmap(bitmapimage);
            //tinted.WritePixels(new Int32Rect(0, 0, bitmapimage.PixelWidth, bitmapimage.PixelHeight), mask, width + 4, 0);
            //return tinted;
            //bitmapimage = LoadImage1(mask);
            //return bitmapimage;
        }

        public BitmapSource FromArray(byte[] data, int w, int h, int ch)
        {
            PixelFormat format = PixelFormats.Default;

            if (ch == 1) format = PixelFormats.Gray8; //grey scale image 0-255
            if (ch == 3) format = PixelFormats.Bgr24; //RGB
            if (ch == 4) format = PixelFormats.Bgr32; //RGB + alpha


            WriteableBitmap wbm = new WriteableBitmap(w, h, 96, 96, format, null);
            wbm.WritePixels(new Int32Rect(0, 0, w, h), data, ch * w, 0);

            return wbm;
        }
        
        public BitmapSource CutEdge(BitmapSource img, int mode, ref int ledge, ref int redge, ref int tedge, ref int bedge)
        {
            int width, height;
            width = img.PixelWidth;
            height = img.PixelHeight;
            int[] xmax;
            int[] ymax;
            byte[] pixels;
            int arrayLength = 0;

            if (width <= 0 || height <= 0)
            {
                return null;
            }

            xmax = new int[width];
            ymax = new int[height];
            arrayLength = width * height;
            pixels = new byte[arrayLength];

            if (img.Format != PixelFormats.Gray8)
            {
                img = new FormatConvertedBitmap(img, PixelFormats.Gray8, null, 0);
            }

            img.CopyPixels(pixels, width, 0);

            //for (int i = 0; i < height; i++)
            System.Threading.Tasks.Parallel.For(0, height, delegate(int i)
            {
                int max = 0;
                for (int j = 0; j < width; j++)
                {
                    if (pixels[i * width + j] > max)
                        max = pixels[i * width + j];
                }
                ymax[i] = max;
            });

            //for (int i = 0; i < width; i++)
            System.Threading.Tasks.Parallel.For(0, width, delegate(int i)
            {
                int max = 0;
                for (int j = 0; j < height; j++)
                {
                    if (pixels[j * width + i] > max)
                        max = pixels[j * width + i];
                }
                xmax[i] = max;
            });

            int lx = 0, rx = 0, uy = 0, by = 0;
            int background = 50;
            //try
            //{
            //    background = Int32.Parse(BackgroundtextBox.Text);
            //}
            //catch (Exception ex)
            //{
            //    logbuffer += ex.Message.ToString();
            //    background = 10;
            //}

            for (int i = width - 1; i >= 0; i--)
                if (xmax[i] > background)
                {
                    rx = (i < width - 1) ? i + 1 : width - 1;
                    break;
                }

            for (int i = 0; i < width; i++)
                if (xmax[i] > background)
                {
                    lx = (i > 0) ? i - 1 : 0;
                    break;
                }

            for (int i = height - 1; i >= 0; i--)
                if (ymax[i] > background)
                {
                    by = (i < height - 1) ? i + 1 : height - 1;
                    break;
                }

            for (int i = 0; i < height; i++)
            {
                if (ymax[i] > background)
                {
                    uy = (i > 0) ? i - 1 : 0;
                    break;
                }
            }
            Int32Rect rect;
            rect = new Int32Rect(lx, uy, rx - lx, by - uy);

            BitmapSource newimg = new CroppedBitmap(img, rect);
            return newimg;
            //ImageWindow bWin = new ImageWindow("Remove blank image");
            //bWin.Show();
            //bWin.imgPhoto.Source = newimg;
            //g_CurrentWindow.imgPhoto.Source = newimg;
        }

        /*
         * 
         */
        public BitmapSource CycleToSquare()
        {
            Bitmap bitmap;
            int width, height;
            byte[] pixels;
            byte[] mask;

            int arrayLength = 0;
            BitmapSource img;

            img = FringesImage;
            if (img.Format != PixelFormats.Gray8)
            {
                img = new FormatConvertedBitmap(img, PixelFormats.Gray8, null, 0);
            }

            //Int32Rect rect = new Int32Rect(u0 - (int)R, v0 - (int)R, 2 * (int)R, 2 * (int)R);
            //BitmapSource newimg = new CroppedBitmap(img, rect);
            BitmapSource newimg = img;
            bitmap = ImageSourceToBitmap(newimg);

            width = bitmap.Width;
            height = bitmap.Height;
            double R = (double)width / 2.0;

            arrayLength = width * height;
            pixels = new byte[arrayLength];
            mask = new byte[arrayLength];
            newimg.CopyPixels(pixels, width, 0);

            // cx = x * Math.sqrt(1-y * y/2)
            // cy = y * Math.sqrt(1-x * x/2)
            for (int i = 0; i < arrayLength; i++)   // mask initial
                mask[i] = 0;

            double x=0,y=0;
            for(double u = 0; u<2*R; u++)
            //System.Threading.Tasks.Parallel.For( 0, 2*R, delegate( double u )
                for (double v = 0; v < 2*R; v++)
                {
                    x = Math.Round(((u/R-1.0) * Math.Sqrt(1.0 - 0.5 * Math.Pow((v/R-1.0),2.0)))*R+R);
                    y = Math.Round(((v/R-1.0) * Math.Sqrt(1.0 - 0.5 * Math.Pow((u/R-1.0),2.0)))*R+R);
                    mask[(int)u + (int)v * width] = pixels[(int)x + (int)y * width];
                }

            WriteableBitmap tinted = new WriteableBitmap(img);
            tinted.WritePixels(new Int32Rect(0, 0, width, height), mask, width, 0);
            return tinted;
        }

        public BitmapSource SquareToCycle()
        {
            Bitmap bitmap;
            int width, height;
            byte[] pixels;
            byte[] mask;

            int arrayLength = 0;
            BitmapSource img;

            img = FringesImage;
            if (img.Format != PixelFormats.Gray8)
            {
                img = new FormatConvertedBitmap(img, PixelFormats.Gray8, null, 0);
            }

            //Int32Rect rect = new Int32Rect(u0 - (int)R, v0 - (int)R, 2 * (int)R, 2 * (int)R);
            //BitmapSource newimg = new CroppedBitmap(img, rect);
            BitmapSource newimg = img;
            bitmap = ImageSourceToBitmap(newimg);

            width = bitmap.Width;
            height = bitmap.Height;
            double R = (double)width / 2.0;

            arrayLength = width * height;
            pixels = new byte[arrayLength];
            mask = new byte[arrayLength];
            newimg.CopyPixels(pixels, width, 0);

            // cx = x * Math.sqrt(1-y * y/2)
            // cy = y * Math.sqrt(1-x * x/2)
            for (int i = 0; i < arrayLength; i++)   // mask initial
                mask[i] = 0;

            double x = 0, y = 0;
            for (double u = 0; u < 2 * R; u++)
                //System.Threading.Tasks.Parallel.For( 0, 2*R, delegate( double u )
                for (double v = 0; v < 2 * R; v++)
                {
                    x = Math.Round(((u / R - 1.0) * Math.Sqrt(1.0 - 0.5 * Math.Pow((v / R - 1.0), 2.0))) * R + R);
                    y = Math.Round(((v / R - 1.0) * Math.Sqrt(1.0 - 0.5 * Math.Pow((u / R - 1.0), 2.0))) * R + R);
                    mask[(int)u + (int)v * width] = pixels[(int)x + (int)y * width];
                }

            WriteableBitmap tinted = new WriteableBitmap(img);
            tinted.WritePixels(new Int32Rect(0, 0, width, height), mask, width, 0);
            return tinted;
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

        private byte[] getMetaData(Bitmap resizebitmap)
        {
            throw new NotImplementedException();
        }

        private byte[] getMetaData(ref BitmapSource img)
        {
            int width, height;
            width = img.PixelWidth;
            height = img.PixelHeight;
            byte[] pixels;
            int arrayLength = 0;

            if (width <= 0 || height <= 0)
            {
                return null;
            }

            arrayLength = width * height;
            pixels = new byte[arrayLength];

            if (img.Format != PixelFormats.Gray8)
            {
                img = new FormatConvertedBitmap(img, PixelFormats.Gray8, null, 0);
            }

            img.CopyPixels(pixels, width, 0);

            return pixels;
        }

        private byte[] getMetaData(ref Bitmap img)
        {
            Byte[] data;

            using (var memoryStream = new MemoryStream())
            {
                img.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                data = memoryStream.ToArray();
            }
            return data;
        }
    }
}
