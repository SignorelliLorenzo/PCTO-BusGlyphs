using System;
using System.Drawing;
using ZXing;

namespace QrCodeReader
{
    public class Class1
    {
        private static string FindQrCodeInImage(Bitmap bmp)
        {
            //decode the bitmap and try to find a qr code
            var source = new BitmapLuminanceSource(bmp);
            var bitmap = new BinaryBitmap(new HybridBinarizer(source));
            var result = new MultiFormatReader().decode(bitmap);

            //no qr code found in bitmap
            if (result == null)
            {


                throw new Exception("Glifo non trovato");
            }

            //return the found qr code text
            return result.Text;
        }
    }
}
