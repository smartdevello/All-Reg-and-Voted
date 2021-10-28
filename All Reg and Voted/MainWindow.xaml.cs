using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using MessageBox = System.Windows.MessageBox;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using System.Windows.Forms;
using System.ComponentModel;

namespace All_Reg_and_Voted
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Reg_Voted_Renderer renderer = null;
        private List<string> all_precincts = null;
        string exportFolderPath = "";

        public MainWindow()
        {
            renderer = null;
            all_precincts = null;
            InitializeComponent();

            pbStatus.Visibility = Visibility.Hidden;
            pbStatus.Value = 0;

        }
        public BitmapSource CreateBitmapFromControl(FrameworkElement element)
        {
            // Get the size of the Visual and its descendants.
            Rect rect = VisualTreeHelper.GetDescendantBounds(element);
            DrawingVisual dv = new DrawingVisual();

            using (DrawingContext ctx = dv.RenderOpen())
            {
                VisualBrush brush = new VisualBrush(element);
                ctx.DrawRectangle(brush, null, new Rect(rect.Size));
            }

            // Make a bitmap and draw on it.
            int width = (int)element.ActualWidth;
            int height = (int)element.ActualHeight;
            RenderTargetBitmap rtb = new RenderTargetBitmap(
                width, height, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(dv);
            return rtb;
        }

        private BitmapImage BmpImageFromBmp(Bitmap bmp)
        {
            using (var memory = new System.IO.MemoryStream())
            {
                bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
        private void SaveControlImage(FrameworkElement control, string filename)
        {
            RenderTargetBitmap rtb = (RenderTargetBitmap)CreateBitmapFromControl(control);
            // Make a PNG encoder.
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            // Save the file.
            using (FileStream fs = new FileStream(filename,
                FileMode.Create, FileAccess.Write, FileShare.None))
            {
                encoder.Save(fs);
            }
        }
        private void btnExportAllChart_Click(object sender, RoutedEventArgs e)
        {
            if (renderer == null)
            {
                renderer = new Reg_Voted_Renderer((int)myCanvas.ActualWidth, (int)myCanvas.ActualHeight);
            }
            if (renderer.getData2008Count() > 0 && renderer.getData2012Count() > 0 && renderer.getData2016Count() > 0 && renderer.getData2020Count() > 0)
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    DialogResult result = dialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                    {
                        exportFolderPath = dialog.SelectedPath;
                        pbStatus.Visibility = Visibility.Visible;
                        pbStatus.Minimum = 0;
                        pbStatus.Maximum = all_precincts.Count;
                        pbStatus.Value = 0;

                        BackgroundWorker worker = new BackgroundWorker();
                        worker.WorkerReportsProgress = true;
                        worker.DoWork += worker_DoExport;
                        worker.ProgressChanged += worker_ProgressChanged;
                        worker.RunWorkerAsync();
                        worker.RunWorkerCompleted += worker_CompletedWork;
                    }
                }
            }
        }
        void worker_DoExport(object sender, DoWorkEventArgs e)
        {
            int index = 0;
            foreach (var precinct in all_precincts)
            {
                try
                {
                    string filename = exportFolderPath + "/" + renderer.draw(precinct) + ".png";
                    SaveBitmapImagetoFile(BmpImageFromBmp(renderer.getBmp()), filename);
                    index++;
                    (sender as BackgroundWorker).ReportProgress(index);
                }
                catch(Exception)
                {

                }
            }

        }
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
        }
        void worker_CompletedWork(object sender, RunWorkerCompletedEventArgs e)
        {
            pbStatus.Visibility = Visibility.Hidden;
            string msg = "Exporting has been done\n";
            MessageBox.Show(msg);
        }
        private void btnExportCurrentChart_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image file (*.png)|*.png";
            //saveFileDialog.Filter = "Image file (*.png)|*.png|PDF file (*.pdf)|*.pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                SaveControlImage(PrecinctChart, saveFileDialog.FileName);
            }
        }
        private void SaveBitmapImagetoFile(BitmapImage image, string filePath)
        {
            //PngBitmapEncoder encoder1 = new PngBitmapEncoder();
            //encoder1.Frames.Add(BitmapFrame.Create(image));

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            try
            {
                using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                {
                    encoder.Save(fileStream);
                }
            }
            catch (Exception ex)
            {

            }


        }
        private void myCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            Render("");
        }
        private void precinctChangedEventHandler(object sender, TextChangedEventArgs args)
        {            
            Render(currentPrecinct.Text);
        }
        private void btnImportExcel_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

            
            List<Reg_Voted_Model> data2020 = new List<Reg_Voted_Model>();
            List<Reg_Voted_Model> data2016 = new List<Reg_Voted_Model>();
            List<Reg_Voted_Model> data2012 = new List<Reg_Voted_Model>();
            List<Reg_Voted_Model> data2008 = new List<Reg_Voted_Model>();
            if (all_precincts == null)
                all_precincts = new List<string>();
            else all_precincts.Clear();

            string errMsg = "";
            if (openFileDialog.ShowDialog() == true)
            {

                try
                {
                    IWorkbook workbook = null;
                    string fileName = openFileDialog.FileName;
                    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        if (fileName.IndexOf(".xlsx") > 0)
                            workbook = new XSSFWorkbook(fs);
                        else if (fileName.IndexOf(".xls") > 0)
                            workbook = new HSSFWorkbook(fs);

                    }
                    for (int sheetIndx = 0; sheetIndx < 4; sheetIndx++)
                    {
                        ISheet sheet = workbook.GetSheetAt(sheetIndx);
                        if (sheet != null)
                        {
                            int rowCount = sheet.LastRowNum;
                            for (int i = 1; i < rowCount; i++)
                            {
                                IRow curRow = sheet.GetRow(i);
                                if (curRow == null)
                                {
                                    rowCount = i - 1;
                                    break;
                                }
                                if (curRow.Cells.Count == 5)
                                {
                                    try
                                    {
                                        string precinct_name = curRow.GetCell(1).StringCellValue.Trim();
                                        var tmp = new Reg_Voted_Model()
                                        {
                                            number = curRow.GetCell(0).StringCellValue.Trim(),
                                            precinct_name = precinct_name,
                                            reg_v = Convert.ToInt32(curRow.GetCell(2).NumericCellValue),
                                            voted = Convert.ToInt32(curRow.GetCell(3).NumericCellValue),
                                            result = curRow.GetCell(4).StringCellValue,
                                        };

                                        if (!all_precincts.Contains(precinct_name))
                                            all_precincts.Add(precinct_name);
                                        if (sheet.SheetName.Contains("2020"))
                                        {
                                            data2020.Add(tmp);
                                        }
                                        else if (sheet.SheetName.Contains("2016"))
                                        {
                                            data2016.Add(tmp);
                                        }
                                        else if (sheet.SheetName.Contains("2012"))
                                        {
                                            data2012.Add(tmp);
                                        }
                                        else if (sheet.SheetName.Contains("2008"))
                                        {
                                            data2008.Add(tmp);
                                        }
                                    }
                                    catch(Exception rowEx)
                                    {

                                    }

                                }
                            }
                        }
                    }


                } catch(Exception ex)
                {
                    string msg = ex.GetType().FullName;
                    if (msg == "System.IO.IOException")
                        MessageBox.Show("The file is open by another process", "Error");
                    else if (msg == "CsvHelper.TypeConversion.TypeConverterException")
                    {
                        MessageBox.Show("The file format is invalid, Please check your csv file again.", "Error");
                    }
                    else if (msg == "CsvHelper.HeaderValidationException")
                    {
                        MessageBox.Show("CSV Header is not correct, please make sure you are using the correct CSV file", "Error");
                    }
                    else
                    {

                    }
                }

                if (!string.IsNullOrEmpty(errMsg))
                {
                    //MessageBox.Show(errMsg, "Error");
                }
                if (data2020.Count > 0 && data2016.Count> 0 && data2012.Count > 0 && data2008.Count> 0)
                {
                    renderer.setData(data2020, data2016, data2012, data2008);                    
                    Render("");
                }

            }
        }
        void Render(string precinct)
        {
            if (renderer == null)
                renderer = new Reg_Voted_Renderer((int)myCanvas.ActualWidth, (int)myCanvas.ActualHeight);
            renderer.setRenderSize((int)myCanvas.ActualWidth, (int)myCanvas.ActualHeight);
            renderer.draw(precinct);
            myImage.Source = BmpImageFromBmp(renderer.getBmp());

        }
    }
}
