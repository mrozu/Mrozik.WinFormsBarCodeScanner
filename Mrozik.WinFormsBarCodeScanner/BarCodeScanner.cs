using System.Windows.Forms;

namespace Mrozik.WinFormsBarCodeScanner
{
    public class BarCodeScanner
    {
        public string Show()
        {
            var form = new BarCodeScannerForm();
            return form.ShowDialog() == DialogResult.OK ? form.RecognizedCode : "";
        }
    }
}