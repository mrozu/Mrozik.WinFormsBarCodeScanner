using System.Collections.Generic;
using System.Drawing;
using ZXing;
using ZXing.Common;
using ZXing.Common.Detector;

namespace Mrozik.WinFormsBarCodeScanner
{
    internal class Code39BarcodeReader : BarcodeReader
    {
        public Code39BarcodeReader()
        {
            Options = new DecodingOptions()
            {
                PossibleFormats = new List<BarcodeFormat>
                {
                    BarcodeFormat.CODE_39
                }
            };

        }

        public void DrawResultRectangle(Bitmap bitmap, ResultPoint[] resultPoints)
        {
            var barcodeRectangle = FindBarcodeRectangle(bitmap, resultPoints);

            using (var g = Graphics.FromImage(bitmap))
            {
                using (var pen = new Pen(Color.Red, 2))
                {
                    g.DrawRectangle(pen, barcodeRectangle);
                }
            }
        }

        private static Rectangle FindBarcodeRectangle(Bitmap bitmap, ResultPoint[] resultPoints)
        {
            var startX = resultPoints[0].X;
            float startY;

            var width = resultPoints[1].X - resultPoints[0].X;
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
                startY = resultPoints[0].Y;
            }

            var barcodeRectangle = new Rectangle((int)startX, (int)startY, (int)width, (int)height);
            return barcodeRectangle;
        }
    }
}