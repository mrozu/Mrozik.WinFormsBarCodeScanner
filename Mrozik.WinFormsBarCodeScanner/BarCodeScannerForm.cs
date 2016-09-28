using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;

namespace Mrozik.WinFormsBarCodeScanner
{
    internal partial class BarCodeScannerForm : Form
    {
        private const int CurrentCameraIndex = 1;
        private readonly Code39BarcodeReader _barcodeReader = new Code39BarcodeReader();
        private VideoCaptureDevice _videoDevice;
        private Bitmap _oldImage;
        private string _previouslyRecognizedBarcode;
        private int _sameBarcodeRepeatTime;

        public string RecognizedCode { get; private set; }

        public BarCodeScannerForm()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            ShowBusyIndicator();
            Task.Factory.StartNew(ConnectToVideoDevice);
        }

        private void ConnectToVideoDevice()
        {
            var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                CloseWithError("No video device found.");
            }
            else
            {
                _videoDevice = new VideoCaptureDevice(videoDevices[CurrentCameraIndex].MonikerString);
                _videoDevice.VideoResolution = _videoDevice.VideoCapabilities[2];
                _videoDevice.NewFrame += VideoDevice_NewFrame;

                _videoDevice.Start();
            }
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

                var recognitionResult = _barcodeReader.Decode(currentImage);
                if (recognitionResult != null)
                {
                    _barcodeReader.DrawResultRectangle(currentImage, recognitionResult.ResultPoints);
                    SetRecognizedBarcode(recognitionResult.Text);
                }
                HideBusyIndicator();
                pictureBox.Image = currentImage;

                _oldImage?.Dispose();
                _oldImage = currentImage;
            }
            catch (Exception ex)
            {
                CloseWithError(ex.Message);
            }
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

        private void SetRecognizedBarcode(string recognizedBarcode)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
               {
                   SetRecognizedBarcode(recognizedBarcode);
               });
            }
            else
            {
                if (_previouslyRecognizedBarcode == recognizedBarcode)
                {
                    _sameBarcodeRepeatTime++;
                }
                else
                {
                    _sameBarcodeRepeatTime = 0;
                    _previouslyRecognizedBarcode = recognizedBarcode;
                }

                if (_sameBarcodeRepeatTime >= 5)
                {
                    RecognizedCode = recognizedBarcode;
                    DialogResult = DialogResult.OK;
                    Close();
                }
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

        private void CloseWithError(string message)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) delegate
                {
                    CloseWithError(message);
                });
            }
            else
            {
                MessageBox.Show(message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
    }
}
