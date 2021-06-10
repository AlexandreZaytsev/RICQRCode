using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

using MessagingToolkit.Barcode;
using MessagingToolkit.Barcode.QRCode.Decoder;
//using MessagingToolkit.Barcode.Pdf417.Encoder;
//using MessagingToolkit.Barcode.QRCode;
//using MessagingToolkit.Barcode.Helper;
//using MessagingToolkit.Barcode.Common;
//using MessagingToolkit.Barcode.Client.Results;
//using MessagingToolkit.Barcode.Multi;

namespace RicQRCode
{
    class Program
    {
        private const BarcodeFormat DefaultBarcodeFormat = BarcodeFormat.QRCode;
//        private const ErrorCorrectionLevel DefaultBarcodeErrorCorrectionLevel = MessagingToolkit.Barcode.QRCode.Decoder.ErrorCorrectionLevel.M;
        private const String DefaultImageFormat = "PNG";
        private const String DefaultOutputFile = "out";
        private const int DefaultWidth = 300;
        private const int DefaultHeight = 300;
        private const int DefaultSize = 1;
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                PrintUsage();
                return;
            }

            BarcodeFormat barcodeFormat = DefaultBarcodeFormat;
            String imageFormat = DefaultImageFormat;
            String outFileString = DefaultOutputFile;
            int width = DefaultWidth;
            int height = DefaultHeight;
            int size = DefaultSize;

            foreach (String arg in args)
            {
                if (arg.StartsWith("--barcode_format", StringComparison.OrdinalIgnoreCase))
                {
                    barcodeFormat = (BarcodeFormat)Enum.Parse(typeof(BarcodeFormat), arg.Split('=')[1].Trim());
                }
                else if (arg.StartsWith("--image_format", StringComparison.OrdinalIgnoreCase))
                {
                    imageFormat = arg.Split('=')[1];
                }
                else if (arg.StartsWith("--output", StringComparison.OrdinalIgnoreCase))
                {
                    outFileString = arg.Split('=')[1];
                }
                else if (arg.StartsWith("--width", StringComparison.OrdinalIgnoreCase))
                {
                    width = Convert.ToInt32(arg.Split('=')[1]);
                }
                else if (arg.StartsWith("--height", StringComparison.OrdinalIgnoreCase))
                {
                    height = Convert.ToInt32(arg.Split('=')[1]);
                }
                else if (arg.StartsWith("--size", StringComparison.OrdinalIgnoreCase))
                {
                    size = Convert.ToInt32(arg.Split('=')[1]);
                }
            }

            //            if (DefaultOutputFile.Equals(outFileString, StringComparison.OrdinalIgnoreCase))
            //            {
            //                outFileString += '.' + imageFormat.ToLower();
            //            }

            String contents = null;
            foreach (String arg in args)
            {
                if (!arg.StartsWith("--"))
                {
                    contents = arg;
                    break;
                }
            }

            if (contents == null)
            {
                PrintUsage();
                return;
            }

            BarcodeEncoder barcodeEncoder = new BarcodeEncoder();
            barcodeEncoder.Content = contents;
            barcodeEncoder.CharacterSet = "UTF-8";
            barcodeEncoder.Width = width;
            barcodeEncoder.Height = height;
            barcodeEncoder.Margin = size;
            barcodeEncoder.ErrorCorrectionLevel = MessagingToolkit.Barcode.QRCode.Decoder.ErrorCorrectionLevel.H;//.M;


            Image image = barcodeEncoder.Encode(barcodeFormat, contents);
            barcodeEncoder.Dispose();

            ImageFormat saveImageFormat = ImageFormat.Png;
            switch (imageFormat.ToLower())
            {
                case "png":
                    saveImageFormat = ImageFormat.Png;
                    break;
                case "jpeg":
                case "jpg":
                    saveImageFormat = ImageFormat.Jpeg;
                    break;
                case "bmp":
                    saveImageFormat = ImageFormat.Bmp;
                    break;
                case "emf":
                    saveImageFormat = ImageFormat.Emf;
                    break;
                case "gif":
                    saveImageFormat = ImageFormat.Gif;
                    break;
                case "icon":
                    saveImageFormat = ImageFormat.Icon;
                    break;
                case "wmf":
                    saveImageFormat = ImageFormat.Wmf;
                    break;
                case "tiff":
                    saveImageFormat = ImageFormat.Tiff;
                    break;
            }
            image.Save(outFileString + "." + imageFormat.ToLower(), saveImageFormat);

        }

        private static void PrintUsage()
        {
            Console.WriteLine("Encodes barcode images using the library\n");
            Console.WriteLine("usage: CommandLineEncoder [ options ] content_to_encode");
            //            Console.WriteLine("  --barcode_format=format: Format to encode, from BarcodeFormat class. " +
            //                                   "Not all formats are supported. Defaults to QR_CODE.");
            Console.WriteLine("  --image_format=format: image output format, such as PNG, JPG, GIF. Defaults to PNG");
            Console.WriteLine("  --output=filename: File to write to. Defaults to out.png");
            Console.WriteLine("  --width=pixels: Image width. Defaults to 300");
            Console.WriteLine("  --height=pixels: Image height. Defaults to 300");
            Console.WriteLine("  --size=integer: Size barcode inside the image (0-10). Defaults to 1");
        }


    }
}
