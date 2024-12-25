namespace QRGenerator.Models
{
    public class QRCodeWithText
    {
        public string QRCodeBase64 { get; set; } // QR Kodunun Base64 formatındaki görseli
        public string Text { get; set; } // Metin
    }

    public class TextToQRModel
    {
        public string InputText { get; set; } // Tek bir textarea'dan alınacak metin
        public List<QRCodeWithText> QRCodeImagesBase64 { get; set; } = new List<QRCodeWithText>();
    }
}
