using CommandLine;
using CommandLine.Text;
using MessagingToolkit.Barcode;
using MessagingToolkit.Barcode.QRCode.Decoder;
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
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;


namespace RicQRCode
{
    class Program
    {
        
        private static readonly Dictionary<string, ErrorCorrectionLevel> ErrorCorrectionLevels =
                        new Dictionary<string, ErrorCorrectionLevel>
                        {
                             { "L", ErrorCorrectionLevel.L},
                             { "M", ErrorCorrectionLevel.M},
                             { "Q", ErrorCorrectionLevel.Q},
                             { "H", ErrorCorrectionLevel.H},
                        };
        
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
                h.Heading = "RicQRCode 1.0.0 (uses two libraries (in the program directory): MessagingToolkit.Barcode.dll, CommandLine.dll)"; //change header
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
            //если имя не пустое и каталог и имя не содержат недопустимых символов)
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

            //файл иконки
            Dictionary<EncodeOptions, object> encodingOptions = new Dictionary<EncodeOptions, object>(1);
            if (!string.IsNullOrEmpty(opts.LogoFileName) && File.Exists(opts.LogoFileName))
            {
                Image logo = Image.FromFile(@opts.LogoFileName);
                encodingOptions.Add(EncodeOptions.QRCodeLogo, logo);
            }
            else
            {
                Console.WriteLine($"{appPath}: {opts.LogoFileName}: No such image logo file or directory");
            }

            GenerateQRCode(opts.Content, opts.EccLevel, opts.OutputFileName, opts.ImageFormat, opts.QrSquareSize, opts.QrMarginSize, opts.ForegroundColor, opts.BackgroundColor, encodingOptions);
            return exitCode;
        }

        //создать QR код
        private static void GenerateQRCode(string payloadString, string eccLevel, string outputFileName, string imgFormat, int pixelQrSquareSize, int MarginSize, string foreground, string background, Dictionary<EncodeOptions, object> encodingOptions)
        {
            using (BarcodeEncoder barcodeEncoder = new BarcodeEncoder())
            {
                try
                {
                    barcodeEncoder.Content = payloadString;
                    barcodeEncoder.CharacterSet = "UTF-8";
                    barcodeEncoder.Width = pixelQrSquareSize;
                    barcodeEncoder.Height = pixelQrSquareSize;
                    barcodeEncoder.Margin = MarginSize;
                    barcodeEncoder.ForeColor = ColorTranslator.FromHtml(foreground);
                    barcodeEncoder.BackColor = ColorTranslator.FromHtml(background);
                    barcodeEncoder.ErrorCorrectionLevel = ErrorCorrectionLevels[eccLevel];

                    //Image image = barcodeEncoder.Encode(BarcodeFormat.QRCode, payloadString);

                    // If there is no encoding options, use
                    //Image image = barcodeEncoder.Encode(BarcodeFormat.QRCode, payloadString);
                    Image image = barcodeEncoder.Encode(BarcodeFormat.QRCode, payloadString, encodingOptions);

                    barcodeEncoder.Dispose();
                    image.Save(outputFileName, new OptionSetter().GetImageFormat(imgFormat));
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"error, so Sorry");
                }
            }
        }

            //прочитать контент из файла
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
                case "bmp":
                    return ImageFormat.Bmp;
                case "emf":
                    return ImageFormat.Emf;
                case "gif":
                    return ImageFormat.Gif;
                case "icon":
                    return ImageFormat.Icon;
                case "wmf":
                    return ImageFormat.Wmf;
                case "tiff":
                    return ImageFormat.Tiff;
                case "png":
                default:
                    return ImageFormat.Png;
            }
        }
    }
}
