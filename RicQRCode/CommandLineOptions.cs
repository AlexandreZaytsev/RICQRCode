using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;

namespace RicQRCode
{
    class Options
    {
        /*
        [Value(index: 0, Required = true, HelpText = "Image file Path to analyze.")]
        public string Path { get; set; }
        */

        [Option(shortName: 'i', longName: "content", Required = true, HelpText = "String or full File name with your content.", Default = null)]
        public string Content { get; set; }

        [Option(shortName: 'o', longName: "outFile", Required = true, HelpText = "Output file. Full file name without extension (extension from outFormat parameter).", Default = null)]
        public string OutputFileName { get; set; }

        [Option(longName: "outFormat", Required = false, HelpText = "Image format for outputfile. Valid values: Png, Jpg, Gif, Bmp, Tiff, Svg", Default = "png")]
        public string ImageFormat { get; set; }

        [Option(longName: "eccLevel", Required = false, HelpText = "Error correction level: L-7%, M-15%, Q-25%, H-30%. Valid values: L, M, Q, H", Default = "L")]
        public string EccLevel { get; set; }
        /*
                [Option(longName: "pixelSize", Required = false, HelpText = "The pixel size each b/w module is drawn (from 1 and more).", Default = 20)]
                public int PixelsPerModule { get; set; }
        */

        [Option(longName: "size", Required = false, HelpText = "Size (px.) of the side of the square of the picture of the QR image.", Default = 300)]
        public int QrSquareSize { get; set; }

        [Option(longName: "margin", Required = false, HelpText = "Size (px.) of the frame around the qr code image (Margin).", Default = 1)]
        public int QrMarginSize { get; set; }

        [Option(longName: "background", Required = false, HelpText = "Background color.", Default = "#000000")]
        public string ForegroundColor { get; set; }

        [Option(longName: "foreground", Required = false, HelpText = "Foreground color.", Default = "#FFFFFF")]
        public string BackgroundColor { get; set; }

        [Option(shortName: 'l', longName: "logoPath", Required = false, HelpText = "Bitmap image logo from file (full file name with extension).", Default = null)]
        public string LogoFileName { get; set; }
/*
        [Option(longName: "iconSize", Required = false, HelpText = "Sets how much 1-99% of the QR Code will be covered by the icon.", Default = 15)]
        public int ImgSize { get; set; }
*/
        [Usage(ApplicationAlias = "RicQRCode.exe")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example>() {
                    new Example("Creates a QR image file (fix imagelSize) from your content (string or file)", new Options { Content="your content", OutputFileName = "your FileName QRImageFile" })
                };
            }
        }
    }
}
