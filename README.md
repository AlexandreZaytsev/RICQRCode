# RicQRCoder
Консольное приложение - создание файла с картинкой QR кода использует библиотеки:
 - генератор картинки QR кода MessagingToolkit.Barcode 1.7.0.1 (http://platform.twit88.com/projects/mt-barcode)
 - парсер командной строки CommandLineParser 2.8.0 (https://github.com/commandlineparser/commandline)

_Windows/собирал под  .NET Framework 4.0_  
_Использование и параметры (RicQRCode.exe --help)_
***
RicQRCode 1.0.0  
Copyright (c) 2021 cad.ru  
USAGE:  
Creates a QR image file from your content (string or file):  
  RicQRCode.exe --content "your content" --outFile "your FileName QRImageFile"

-  -i, --content     Required. String or full File name with your content.
-  -o, --outFile     Required. Output file. Full file name without extension (extension from outFormat parameter).
-  --outFormat       (Default: png) Image format for outputfile. Valid values: Png, Jpg, Gif, Bmp, Tiff, Svg)
-  --eccLevel        (Default: L) Error correction level: L-7%, M-15%, Q-25%, H-30%. Valid values: L, M, Q, H
-  --size            (Default: 300) Size (px.) of the side of the square of the picture of the QR image.
-  --margin          (Default: 1) Size (px.) of the frame around the qr code image (Margin).
-  --background      (Default: #000000) Background color.
-  --foreground      (Default: #FFFFFF) Foreground color.
-  -l, --logoPath    Bitmap image logo from file (full file name with extension).
-  --help            Display this help screen.
-  --version         Display version information.

Good luck...
***
использую для документов MSOffice выпускаемых из CRM - создать файл на диске - вставить в документ в рамку Shape найденную по имени  
например:  
```
    For each Shape in docWord.Shapes ' цикл по всем Shapes документа   
      If ... Then    
        tmp = FSO.FindFile(vPathTemp & "\", Shape.Title, "png") 'попробовать найти файйл QR на диске
        if tmp<>"" then
          Shape.Fill.UserPicture vPathTemp & "\" & tmp  ' если есть вставить ссылку в Shape
       Else
          Shape.Fill.Visible = False    'если нет - загасить Shape 
       End if
     End if
     Shape.Line.Visible = False ' удалить рамку Shape в принципе (типа был обработан)
     Next
```
