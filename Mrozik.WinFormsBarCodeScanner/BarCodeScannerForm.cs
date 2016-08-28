using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;

namespace Mrozik.WinFormsBarCodeScanner
{
    public partial class BarCodeScannerForm : Form
    {
        private int _currentCameraIndex = 1;
        private IBarcodeReader _barcodeReader;
        private VideoCaptureDevice _videoDevice;
        private FilterInfoCollection _videoDevices;

        public BarCodeScannerForm()
        {
            InitializeComponent();
            _barcodeReader = new BarcodeReader();
        }

        protected override void OnShown(EventArgs e)
        {
            try
            {
                ShowBusyIndicator();
                Task.Run(() =>
                {
                    _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                    ConnectToVideoDevice();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ConnectToVideoDevice()
        {
            _videoDevice = new VideoCaptureDevice(_videoDevices[_currentCameraIndex].MonikerString);
            _videoDevice.VideoResolution = _videoDevice.VideoCapabilities[2];
            _videoDevice.NewFrame += VideoDevice_NewFrame;

            _videoDevice.Start();
        }

        private Bitmap _oldImage;

        private void VideoDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                var currentImage = eventArgs.Frame;
                if (currentImage != null)
                {
                    currentImage = (Bitmap)currentImage.Clone();
                    currentImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    var result = _barcodeReader.Decode(currentImage);
                    if (result != null)
                    {
                        SetRecognizedBarcode(result.Text);
                        //var resultPoints = result.ResultPoints;
                        //using (var g = Graphics.FromImage(currentImage))
                        //    g.DrawRectangle(new Pen(Brushes.Red), resultPoints[0].X, resultPoints[2].Y, resultPoints[2].X, resultPoints[0].Y);
                    }
                    HideBusyIndicator();
                    pictureBox1.Image = currentImage;

                    _oldImage?.Dispose();
                    _oldImage = currentImage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
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
            DisconnectVideoDevice();
            base.OnClosed(e);
        }

        private void DisconnectVideoDevice()
        {
            if (_videoDevice != null)
            {
                _videoDevice.SignalToStop();
                _videoDevice.WaitForStop();
                _videoDevice.NewFrame -= VideoDevice_NewFrame;
            }
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

            ShowBusyIndicator();

            Task.Run(() =>
            {
                try
                {
                    DisconnectVideoDevice();
                    ConnectToVideoDevice();
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
