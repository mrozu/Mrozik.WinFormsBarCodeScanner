using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Common;
using ZXing.Common.Detector;

namespace Mrozik.WinFormsBarCodeScanner
{
    public partial class BarCodeScannerForm : Form
    {
        private int _currentCameraIndex = 1;
        private readonly IBarcodeReader _barcodeReader;
        private VideoCaptureDevice _videoDevice;
        private FilterInfoCollection _videoDevices;
        private Bitmap _oldImage;
        private string _barcode;
        private int _sameBarcodeRepeatTime;

        public BarCodeScannerForm()
        {
            InitializeComponent();
            _barcodeReader = new BarcodeReader
            {
                Options =
                {
                    PossibleFormats = new List<BarcodeFormat>
                    {
                        BarcodeFormat.CODE_39
                    },

                }
            };
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

        private static void DrawBarcodeRectangle(Bitmap bitmap, Result result)
        {
            var barcodeRectangle = FindBarcodeRectangle(bitmap, result);

            using (var g = Graphics.FromImage(bitmap))
            {
                using (var pen = new Pen(Color.Red, 2))
                {
                    g.DrawRectangle(pen, barcodeRectangle);
                }
            }
        }

        private static Rectangle FindBarcodeRectangle(Bitmap bitmap, Result result)
        {
            var startX = result.ResultPoints[0].X;
            float startY;

            var width = result.ResultPoints[1].X - result.ResultPoints[0].X;
            float height;


            var luminanceSource = new BitmapLuminanceSource(bitmap);
            var binarizer = new HybridBinarizer(luminanceSource);
            var bitMatrix = binarizer.BlackMatrix;
            var whiteRectangleDetector = WhiteRectangleDetector.Create(bitMatrix);
            var whiteRectanblePoints = whiteRectangleDetector.detect();
            if (whiteRectanblePoints != null)
            {
                height = whiteRectanblePoints[3].Y - whiteRectanblePoints[0].Y;
                startY = whiteRectanblePoints[0].Y;
            }
            else
            {
                height = 1;
                startY = result.ResultPoints[0].Y;
            }

            var barcodeRectangle = new Rectangle((int)startX, (int)startY, (int)width, (int)height);
            return barcodeRectangle;
        }

        private void ConnectToVideoDevice()
        {
            _videoDevice = new VideoCaptureDevice(_videoDevices[_currentCameraIndex].MonikerString);
            _videoDevice.VideoResolution = _videoDevice.VideoCapabilities[2];
            _videoDevice.NewFrame += VideoDevice_NewFrame;

            _videoDevice.Start();
        }


        private void VideoDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                var currentImage = eventArgs.Frame;
                if (currentImage == null)
                    return;

                currentImage = (Bitmap)currentImage.Clone();
                currentImage.RotateFlip(RotateFlipType.Rotate90FlipNone);

                var result = _barcodeReader.Decode(currentImage);
                if (result != null)
                {
                    DrawBarcodeRectangle(currentImage, result);
                    SetRecognizedBarcode(result.Text);
                }
                HideBusyIndicator();
                pictureBox.Image = currentImage;

                _oldImage?.Dispose();
                _oldImage = currentImage;
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
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
               {
                   SetRecognizedBarcode(text);
               });
            }
            else
            {
                if (_barcode == text)
                {
                    _sameBarcodeRepeatTime++;
                }
                else
                {
                    _sameBarcodeRepeatTime = 0;
                    _barcode = text;
                }

                if (_sameBarcodeRepeatTime >= 5)
                {
                    _videoDevice.NewFrame -= VideoDevice_NewFrame;
                    MessageBox.Show(text);
                    _videoDevice.NewFrame += VideoDevice_NewFrame;
                }
            }
        }
    }
}
