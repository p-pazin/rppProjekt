using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using PdfiumViewer;
using PdfSharp.Pdf.Advanced;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCDocumentPreview.xaml
    /// </summary>
    public partial class UCDocumentPreview : UserControl
    {
        public UCDocumentPreview(byte[] pdfData)
        {
            InitializeComponent();
            LoadPdf(pdfData);
        }

        public void LoadPdf(byte[] pdfData)
        {
            string tempFilePath = System.IO.Path.GetTempFileName() + ".pdf";
            File.WriteAllBytes(tempFilePath, pdfData);

            PdfDocument pdfDocument = PdfDocument.Load(tempFilePath);

            var pageImage = pdfDocument.Render(0, 300, 300, true);

            var bitmapSource = ConvertToBitmapSource((Bitmap)pageImage);

            PdfImageControl.Source = bitmapSource;
        }

        private BitmapSource ConvertToBitmapSource(Bitmap drawingBitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                drawingBitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                return BitmapFrame.Create(memory);
            }
        }
    }
}
