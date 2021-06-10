using CommandLine;
using CommandLine.Text;
using MessagingToolkit.Barcode;
//using MessagingToolkit.Barcode.QRCode.Decoder;
//using MessagingToolkit.Barcode.Pdf417.Encoder;
//using MessagingToolkit.Barcode.QRCode;
//using MessagingToolkit.Barcode.Helper;
//using MessagingToolkit.Barcode.Common;
//using MessagingToolkit.Barcode.Client.Results;
//using MessagingToolkit.Barcode.Multi;
using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Drawing;
using System.Drawing.Imaging;


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
            var parser = new CommandLine.Parser(with => with.HelpWriter = null);
            var parserResult = parser.ParseArguments<Options>(args);
            parserResult
                .WithParsed<Options>(options => RunOptionsAndReturnExitCode(options))
                .WithNotParsed(errs => DisplayHelp(parserResult, errs));
        }

        //in case of errors or --help or --version
        static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AdditionalNewLineAfterOption = false; //remove the extra newline between options
                h.AddEnumValuesToHelpText = true;
                h.MaximumDisplayWidth = 200;
                h.Heading = "RicQRCoder 1.0.0"; //change header
                h.Copyright = "Copyright (c) 2021 cad.ru"; //change copyrigt text
                h.AddPreOptionsLine("");// ("<<license as is>>");
                h.AddPostOptionsText("Good luck...");
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);
            Console.WriteLine(helpText);
        }

        //In sucess: the main logic to handle the options
        static int RunOptionsAndReturnExitCode(Options opts)
        {
            Bitmap ImgBitmap = null;
            var exitCode = 0;
            string appPath = AppDomain.CurrentDomain.BaseDirectory; //string yourpath = Environment.CurrentDirectory (нет закрывающего слеша)
                                                                    //            Console.WriteLine("props= {0}", string.Join(",", props));

            //загрузка контента
            if (opts.Content != null)
            {
                if (File.Exists(opts.Content))                                          //если файл существует
                {
                    opts.Content = GetTextFromFile(new FileInfo(opts.Content));         //читаем из файла
                }
                else
                {
                    opts.Content = opts.Content;                                        //иначе читаем из строкового параметра                                         
                }
            }
            else
            {
                opts.Content = "Content not defined";
            }

            //файл QR
            //если имя не пустое и каталог и имя не содержат недопустимыз символов)
            if (opts.OutputFileName != null)// && ((opts.OutputFileName.IndexOfAny(Path.GetInvalidPathChars()) == -1) && (opts.OutputFileName.IndexOfAny(Path.GetInvalidFileNameChars()) == -1)))
            {
                //удалить расширение если есть и добавить из параметров
                if (Path.HasExtension(opts.OutputFileName))                          //если расширения есть - удалим его
                {
                    opts.OutputFileName = Path.GetDirectoryName(opts.OutputFileName) + Path.GetFileNameWithoutExtension(opts.OutputFileName);
                }
                opts.OutputFileName += "." + opts.ImageFormat.ToString().ToLower();

                //если каталог не существует
                if ((!Directory.Exists(Path.GetDirectoryName(opts.OutputFileName))))
                {
                    opts.OutputFileName = appPath + Path.GetFileName(opts.OutputFileName);
                }
            }
            else
            {
                opts.OutputFileName = appPath + "QRImageFile." + opts.ImageFormat.ToString().ToLower();
            }



            return exitCode;
        }
            /*
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

                        using (BarcodeEncoder barcodeEncoder = new BarcodeEncoder())
                        {
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
                    }
            */
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

        private static string GetTextFromFile(FileInfo fileInfo)
        {
            var buffer = new byte[fileInfo.Length];

            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open))
            {
                fileStream.Read(buffer, 0, buffer.Length);
            }
            return Encoding.UTF8.GetString(buffer);
        }

    }
    public class OptionSetter
    {
 //       public QRCodeGenerator.ECCLevel GetECCLevel(string value)
 //       {
 //           Enum.TryParse(value, out QRCodeGenerator.ECCLevel level);
 //           return level;
 //       }

        public ImageFormat GetImageFormat(string value)
        {
            switch (value.ToLower())
            {
                case "jpeg":
                case "jpg":
                    return ImageFormat.Jpeg;
                    break;
                case "bmp":
                    return ImageFormat.Bmp;
                    break;
                case "emf":
                    return ImageFormat.Emf;
                    break;
                case "gif":
                    return ImageFormat.Gif;
                    break;
                case "icon":
                    return ImageFormat.Icon;
                    break;
                case "wmf":
                    return ImageFormat.Wmf;
                    break;
                case "tiff":
                    return ImageFormat.Tiff;
                    break;
                case "png":
                default:
                    return ImageFormat.Png;
            }
        }
    }
}
