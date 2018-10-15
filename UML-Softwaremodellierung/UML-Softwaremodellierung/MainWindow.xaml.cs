using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using iTextSharp;
using System.Diagnostics;
using Syroot.Windows.IO;
using TallComponents.PDF.Rasterizer;

namespace UML_Softwaremodellierung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Local variables

        private double zoomvalue = 1.0; // Vergrößerungsvalue

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;

            //geometry
            wp_geo.Children.Add(new RectangleTool("rectangle"));
            wp_geo.Children.Add(new CircleTool("circle"));
            wp_geo.Children.Add(new TriangleTool("triangle"));
            wp_geo.Children.Add(new CuboidTool("cuboid"));

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Scrollviewer magnifying
            sv_main.ScrollToHorizontalOffset(Convert.ToDouble((sv_main.ScrollableWidth / 2)));
            fillcb();

        }

        #region letteroptions
       
        private void chb_grid_Checked(object sender, RoutedEventArgs e) //Grid on/off
        {
            if (chb_grid.IsChecked == true)
            {
                gr_grid.Visibility = Visibility.Visible;
                cv_Modell.Background = Brushes.Transparent;
            }           
            else
            {
                gr_grid.Visibility = Visibility.Hidden;
                cv_Modell.Background = Brushes.White;
            }
               

        }

        private void chb_grid_middleline_Checked(object sender, RoutedEventArgs e) //middleline on/off
        {
            if (chb_grid_middleline.IsChecked == true)
                bor_middleline.Visibility = Visibility.Visible;
            else
                bor_middleline.Visibility = Visibility.Hidden;

        }

        #endregion

        #region magnifying

        private void fillcb()
        {
            cb_lens.Items.Add("200%");
            cb_lens.Items.Add("175%");
            cb_lens.Items.Add("150%");
            cb_lens.Items.Add("125%");
            cb_lens.Items.Add("100%");
            cb_lens.Items.Add("75%");
            cb_lens.Items.Add("50%");
            cb_lens.Items.Add("25%");

            cb_lens.SelectedIndex = 4;
        }

        private void b_plus_Click(object sender, RoutedEventArgs e)
        {
            if (cb_lens.SelectedIndex > 0)
            {
                cb_lens.SelectedIndex -= 1;

            }

        }

        private void b_minus_Click(object sender, RoutedEventArgs e)
        {
            if (cb_lens.SelectedIndex < 7)
            {
                cb_lens.SelectedIndex += 1;

            }

        }

        private void cb_lens_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_lens.SelectedIndex == 0) zoomvalue = 2.00;
            if (cb_lens.SelectedIndex == 1) zoomvalue = 1.75;
            if (cb_lens.SelectedIndex == 2) zoomvalue = 1.50;
            if (cb_lens.SelectedIndex == 3) zoomvalue = 1.25;
            if (cb_lens.SelectedIndex == 4) zoomvalue = 1.00;
            if (cb_lens.SelectedIndex == 5) zoomvalue = 0.75;
            if (cb_lens.SelectedIndex == 6) zoomvalue = 0.50;
            if (cb_lens.SelectedIndex == 7) zoomvalue = 0.25;

            ScaleTransform scale = new ScaleTransform(zoomvalue, zoomvalue);
            sv_main.LayoutTransform = scale;
        }


        #endregion

        #region dropfunction

        int id = 0;

        private void gr_grid_Drop(object sender, DragEventArgs e) //dropevent
        {          
            Tool _element = (Tool)e.Data.GetData("Object");
            double row, column;

            double x, y;
            y = e.GetPosition(cv_Modell).Y;
            x = e.GetPosition(cv_Modell).X;

            row = Math.Ceiling(y / 8.75);
            column = Math.Ceiling(x / 8.75);

            if (e.AllowedEffects.HasFlag(DragDropEffects.Move) && e.KeyStates != DragDropKeyStates.ControlKey)
            {
                e.Effects = DragDropEffects.Move;

                _element.SetValue(Canvas.LeftProperty, (column * 8.75) - (_element.Width / 2)); // column * 8.75 to align it to the border
                _element.SetValue(Canvas.TopProperty, ((row * 8.75) - (_element.Height / 2)));

            }
            else
            {
                string s = _element.ToString().Split('.')[1]; //Namespace.Class -> 1. classname needed

                if (s == "CircleTool")
                {
                    CircleTool circle = new CircleTool("_" + Convert.ToString(id++));

                    #region not finished
                    //if (_element.Parent is Canvas)
                    //{
                    //    circle = new CircleTool(_element);


                    //    //circle = new  CircleTool { DataContext = _element.DataContext };      //Doesen't work    1
                    //    //circle = XamlReader.Parse(XamlWriter.Save(_element)) as CircleTool;   //Doesent't work either 2
                    //    //circle = (CircleTool)e.Data.GetData("Object");

                    //    //circle = (CircleTool)_element;
                    //    //cv_Modell.Children.Remove(circle);

                    //    //Panel test = (Panel)VisualTreeHelper.GetChild(circle, 1); // 3
                    //    //test.Children.Clear();

                    //    //var xaml = XamlWriter.Save(_element); // 4 
                    //    //var xamlString = new StringReader(xaml);
                    //    //var xmlTextReader = new XmlTextReader(xamlString);
                    //    //circle = (CircleTool)XamlReader.Load(xmlTextReader);
                    //}
                    //else
                    //{
                    //    circle = new CircleTool();

                    //}

                    //TextBox tb = (TextBox)circle.FindName("tb_shape");

                    //tb.IsEnabled = true;
                    #endregion

                    cv_Modell.Children.Add(circle);
                    circle.SetValue(Canvas.LeftProperty,(column * 8.75) - (circle.Width / 2));
                    circle.SetValue(Canvas.TopProperty,((row * 8.75) - (circle.Height / 2)));
                }
                if(s == "CuboidTool")
                {
                    CuboidTool cuboid = new CuboidTool("_" + Convert.ToString(id++));
                    cv_Modell.Children.Add(cuboid);
                    //cuboid.Width = 35; cuboid.Height = 35;

                    cuboid.SetValue(Canvas.LeftProperty, (column * 8.75) - (cuboid.Width / 2));
                    cuboid.SetValue(Canvas.TopProperty, ((row * 8.75) - (cuboid.Height / 2)));
                }
                if(s == "RectangleTool")
                {
                    RectangleTool rectangle = new RectangleTool("_" + Convert.ToString(id++)) { VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left };
                    cv_Modell.Children.Add(rectangle);
                    //circle.Width = 100;

                    rectangle.SetValue(Canvas.LeftProperty, (column * 8.75) - (rectangle.Width / 2));
                    rectangle.SetValue(Canvas.TopProperty, ((row * 8.75) - (rectangle.Height / 2)));
                }
                if(s == "TriangleTool")
                {
                    TriangleTool triangle = new TriangleTool("_" + Convert.ToString(id++)) { VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left };
                    cv_Modell.Children.Add(triangle);

                    triangle.SetValue(Canvas.LeftProperty, (column * 8.75) - (triangle.Width / 2));
                    triangle.SetValue(Canvas.TopProperty, ((row * 8.75) - (triangle.Height / 2)));
                }
                
            }

        }

        private void gr_grid_DragOver(object sender, DragEventArgs e) //dropevent
        {
            if (e.Data.GetDataPresent("Object"))
            {
                // These Effects values are used in the drag source's
                // GiveFeedback event handler to determine which cursor to display.
                if (e.KeyStates == DragDropKeyStates.ControlKey)
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.Move;
                }
            }
        }

        #endregion

        #region xml/pdf/print

        //
        //xml
        //

        private void b_load_Click(object sender, RoutedEventArgs e) //open file
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".xaml";
            dlg.Filter = "Text Document (.xaml)|*.xaml";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                XamlReader xr = new XamlReader();

                cv_Modell.Children.Clear();
                cv_main.Children.Remove(cv_Modell);

                FileStream Fs = new FileStream(dlg.FileName, FileMode.Open);
                Panel copy = (Panel)XamlReader.Load(Fs) as Panel;
                copy.Height = 210;
                copy.Width = 400;

                cv_main.Children.Add(copy);
                copy.BringIntoView();
                //copy.

            }
        }

        private void b_save_Click(object sender, RoutedEventArgs e) //save file
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "new"; // Default file name
                dlg.DefaultExt = ".xaml"; // Default file extension
                dlg.Filter = "Text Document (.xaml)|*.xaml"; // Filter files by extension
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true) name = dlg.FileName;

                string mystrXAML = XamlWriter.Save(cv_Modell);
                FileStream fs = File.Create(dlg.FileName);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(mystrXAML);
                sw.Close();
                fs.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //
        //pdf
        //

        public string name = "";

        private void b_pdf_convert(object sender, RoutedEventArgs e)
        {
            if (name == "")
            {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Document"; 
            dlg.DefaultExt = ".pdf"; 
            dlg.Filter = "Text documents (.pdf)|*.pdf"; // Filter files by extension
            Nullable<bool> result = dlg.ShowDialog();
                if (result == true) name = dlg.FileName;
            }

            if(name !="")
            {       
            gr_grid.Visibility = Visibility.Hidden;
            cv_Modell.Background = Brushes.White;

            string sImagePath = (new KnownFolder(KnownFolderType.Downloads).Path) + "window.png";

            SaveAsPng(GetImage(cv_Modell), sImagePath);
            CreatePdfFromImage(sImagePath, name, true);

            gr_grid.Visibility = Visibility.Visible;
            cv_Modell.Background = Brushes.Transparent;
            }            
        }

        public static RenderTargetBitmap GetImage(UIElement view)
        {
            Size size = new Size(view.RenderSize.Width, view.RenderSize.Height);
            if (size.IsEmpty) return null;
            RenderTargetBitmap result = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Default);       
            DrawingVisual drawingvisual = new DrawingVisual();
            using (DrawingContext context = drawingvisual.RenderOpen())
            {
                context.DrawRectangle(new VisualBrush(view), null, new Rect(0, 0, (int)size.Width, (int)size.Height));            
                context.Close();
            }

            result.Render(drawingvisual);
            return result;
        }

        public static void SaveAsPng(RenderTargetBitmap src, string targetFile)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(src));

            using (var stm = File.Create(targetFile))
            {
                encoder.Save(stm);
            }
        }

        public static void CreatePdfFromImage(string imageFile, string pdfFile, bool bo)
        {
            if (File.Exists(pdfFile))
            {
                File.Delete(pdfFile);
            }

            using (var ms = new MemoryStream())
            {
                var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);
                iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(pdfFile, FileMode.Create));
                iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms).SetFullCompression();
                document.Open();

                FileStream fs = new FileStream(imageFile, FileMode.Open);
                var image = iTextSharp.text.Image.GetInstance(fs);
                image.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
                document.Add(image);
                document.Close();

                if(bo == true)  //save as pdf
                {
                    try
                    {
                        Process.Start("chrome.exe", pdfFile); // if you using firefox just edit it to firefox.exe
                    }
                    catch
                    {
                        Process.Start("explorer.exe", pdfFile);
                    }
                }
                else            //print (the Reallife printer)
                {
                    PrintDialog printDialog = new PrintDialog();
                    printDialog.PageRangeSelection = PageRangeSelection.AllPages;
                    printDialog.UserPageRangeEnabled = true;
                    bool? doPrint = printDialog.ShowDialog();
                    if (doPrint != true)
                    {
                        return;
                    }

                    FixedDocument fixedDocument;
                    using (FileStream pdf = new FileStream(pdfFile, FileMode.Open, FileAccess.Read))
                    {
                        Document doc = new Document(pdf);
                        TallComponents.PDF.Rasterizer.Configuration.RenderSettings renderSettings = new TallComponents.PDF.Rasterizer.Configuration.RenderSettings();
                        ConvertToWpfOptions renderOptions = new ConvertToWpfOptions { ConvertToImages = false };
                        renderSettings.RenderPurpose = TallComponents.PDF.Rasterizer.Configuration.RenderPurpose.Print;
                        renderSettings.ColorSettings.TransformationMode = TallComponents.PDF.Rasterizer.Configuration.ColorTransformationMode.HighQuality;
                        //convert the pdf with the rendersettings and options to a fixed-size document which can be printed
                        fixedDocument = doc.ConvertToWpf(renderSettings, renderOptions);
                    }
                    printDialog.PrintDocument(fixedDocument.DocumentPaginator, "Print");
                }                       
            }
        }

        //
        //print
        //
        
        private void b_print_Click(object sender, RoutedEventArgs e)
        {
            gr_grid.Visibility = Visibility.Hidden;
            cv_Modell.Background = Brushes.White;

            //PDF Converter 
            string sPDFFileName = System.IO.Path.GetTempPath() + "PDFFile.pdf";
            string sImagePath = System.IO.Path.GetTempPath() + "window.png";

            SaveAsPng(GetImage(cv_Modell), sImagePath);             //same as saving to pdf but it will only save it in the temp windows folder
            CreatePdfFromImage(sImagePath, sPDFFileName, false);    //because it will deleted after printing the document.

            gr_grid.Visibility = Visibility.Visible;
            cv_Modell.Background = Brushes.Transparent;


        }

        #endregion
    }
}