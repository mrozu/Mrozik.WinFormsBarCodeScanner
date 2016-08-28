using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Mrozik.WinFormsBarCodeScanner
{
    public partial class BarCodeScannerForm : Form
    {
        private Capture _capture;
        private int _currentCameraIndex = 1;

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
            cameraViewer.Image = image;
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
