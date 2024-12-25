using Microsoft.AspNetCore.Mvc;
using SkiaSharp;
using System.IO;
using QRGenerator.Models;
using QRCoder;

namespace QRGenerator.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new TextToQRModel());
        }

        [HttpPost]
        public IActionResult Index(TextToQRModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.InputText))
            {
                model.QRCodeImagesBase64 = new List<QRCodeWithText>();

                // Kullanýcýdan alýnan metni satýr satýr bölelim
                var lines = model.InputText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        // QR Kodunu üret
                        var qrCodeGenerator = new QRCodeGenerator();
                        var qrCodeData = qrCodeGenerator.CreateQrCode(line, eccLevel: QRCodeGenerator.ECCLevel.L);

                        // QR Kodunu PNG formatýnda kaydet
                        using (var bitmap = new SKBitmap(256, 256))
                        using (var canvas = new SKCanvas(bitmap))
                        {
                            canvas.Clear(SKColors.White);

                            int size = qrCodeData.ModuleMatrix.Count;
                            float scale = 256f / size;

                            for (int y = 0; y < size; y++)
                            {
                                for (int x = 0; x < size; x++)
                                {
                                    if (qrCodeData.ModuleMatrix[y][x])
                                    {
                                        var rect = new SKRect(x * scale, y * scale, (x + 1) * scale, (y + 1) * scale);
                                        canvas.DrawRect(rect, new SKPaint { Color = SKColors.Black });
                                    }
                                }
                            }

                            canvas.Flush();

                            // Görüntüyü PNG formatýnda Base64'e dönüþtür
                            using (var image = SKImage.FromBitmap(bitmap))
                            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    data.SaveTo(memoryStream);
                                    var base64Image = Convert.ToBase64String(memoryStream.ToArray());

                                    // QR kodu ve metni birleþtirerek listemize ekliyoruz
                                    model.QRCodeImagesBase64.Add(new QRCodeWithText
                                    {
                                        QRCodeBase64 = $"data:image/png;base64,{base64Image}",
                                        Text = line
                                    });
                                }
                            }
                        }
                    }
                }
            }

            return View(model);
        }
    }
}
