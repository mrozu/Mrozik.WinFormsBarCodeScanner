using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using ZXing;

namespace Mrozik.WinFormsBarCodeScanner
{
    public partial class BarCodeScannerForm : Form
    {
        private Capture _capture;
        private int _currentCameraIndex = 1;
        private IBarcodeReader _barcodeReader;

        public BarCodeScannerForm()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            try
            {
                Task.Run(() =>
                {
                    ShowBusyIndicator();
                    _capture = new Capture(_currentCameraIndex);
                    _barcodeReader = new BarcodeReader();

                    HideBusyIndicator();
                });

                Application.Idle += ProcessFrame;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void HideBusyIndicator()
        {
            loadingPictureBox.Visible = false;
        }

        private void ShowBusyIndicator()
        {
            loadingPictureBox.Visible = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_capture != null)
            {
                _capture.Dispose();
                _capture = null;
            }
            base.OnClosed(e);
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            if (_capture == null) return;

            var imageframe = _capture.QueryFrame();
            var image = imageframe.ToImage<Rgb, byte>().Rotate(90, new Rgb(Color.Transparent));
            Task.Run(() =>
            {
                var result = _barcodeReader.Decode(image.ToBitmap());
                if (result != null)
                {
                    SetRecognizedBarcode(result.Text);
                    image.DrawPolyline(result.ResultPoints.Select(p => new Point((int)p.X, (int)p.Y)).ToArray(), true, new Rgb(Color.Red), 3);
                }
                cameraViewer.Image = image;
            });
        }

        private void SetRecognizedBarcode(string text)
        {
            if (recognizedBarCode.InvokeRequired)
            {
                recognizedBarCode.Invoke((MethodInvoker)delegate
               {
                   SetRecognizedBarcode(text);

               });
            }
            else
            {
                recognizedBarCode.Text = text;
            }
        }

        private void switchCameraButton_Click(object sender, EventArgs e)
        {
            _currentCameraIndex = _currentCameraIndex == 1 ? 0 : 1;

            Task.Run(() =>
            {
                try
                {
                    ShowBusyIndicator();
                    _capture.Dispose();
                    _capture = null;
                    _capture = new Capture(_currentCameraIndex);
                    HideBusyIndicator();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    throw;
                }
            });
        }
    }
}
