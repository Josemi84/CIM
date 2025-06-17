using ZXing.Net.Maui;

namespace CIM.APP
{
    public partial class BarcodeScannerPage : ContentPage
    {
        bool isScanning = true;

        public BarcodeScannerPage()
        {
            InitializeComponent();
        }

        private async void OnBarcodeDetected(object sender, BarcodeDetectionEventArgs e)
        {
            if (!isScanning)
                return;

            // Solo permitir códigos de barras lineales
            var allowedFormats = new[]
            {
                BarcodeFormat.Codabar,
                BarcodeFormat.Code39,
                BarcodeFormat.Code93,
                BarcodeFormat.Code128,
                BarcodeFormat.Ean8,
                BarcodeFormat.Ean13,
                BarcodeFormat.Itf,
                BarcodeFormat.Rss14,
                BarcodeFormat.RssExpanded,
                BarcodeFormat.UpcA,
                BarcodeFormat.UpcE,
                BarcodeFormat.UpcEanExtension
            };

            var result = e.Results?.FirstOrDefault(r => allowedFormats.Contains(r.Format));

            if (result == null)
                return;

            isScanning = false;

            await Dispatcher.DispatchAsync(async () =>
            {
                string quantityStr = await DisplayPromptAsync("Cantidad", $"Código escaneado: {result.Value}\nIntroduce la cantidad:", "Aceptar", "Cancelar", keyboard: Keyboard.Numeric);
                if (int.TryParse(quantityStr, out int quantity) && quantity > 0)
                {
                    MessagingCenter.Send(this, "BarcodeScanned", new ScannedItem { Barcode = result.Value, Quantity = quantity });
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Cantidad no válida.", "OK");
                    isScanning = true;
                }
            });
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }

    public class ScannedItem
    {
        public string Barcode { get; set; }
        public int Quantity { get; set; }
    }
}
