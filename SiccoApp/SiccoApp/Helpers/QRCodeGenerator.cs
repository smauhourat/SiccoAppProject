using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using ZXing;
using ZXing.QrCode;

namespace SiccoApp.Helpers
{
    public static class QRCodeGenerator
    {
        public static string GenerateQRCodeInMemory(string qrcodeText)
        {
            var imagePath = "";
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.PDF_417,
                Options = new QrCodeEncodingOptions { Margin = 1 }
            };
            //barcodeWriter.Format = BarcodeFormat.QR_CODE;
            //barcodeWriter.Format = BarcodeFormat.PDF_417;
            var result = barcodeWriter.Write(qrcodeText);

            var barcodeBitmap = new Bitmap(result);

            using (MemoryStream memory = new MemoryStream())
            {
                barcodeBitmap.Save(memory, ImageFormat.Png);
                imagePath = "data:image/png;base64," + Convert.ToBase64String(memory.ToArray());
            }

            return imagePath;
        }
    }
}