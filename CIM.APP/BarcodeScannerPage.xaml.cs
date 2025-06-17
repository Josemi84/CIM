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

            isScanning = false;

            var result = e.Results?.FirstOrDefault()?.Value;
            if (result != null)
            {
                await Dispatcher.DispatchAsync(async () =>
                {
                    // Mostrar prompt para cantidad después de escanear
                    string quantityStr = await DisplayPromptAsync("Cantidad", $"Código escaneado: {result}\nIntroduce la cantidad:", "Aceptar", "Cancelar", keyboard: Keyboard.Numeric);
                    if (int.TryParse(quantityStr, out int quantity) && quantity > 0)
                    {
                        // Enviar el código y cantidad a la página MainPage (u otra lógica)
                        MessagingCenter.Send(this, "BarcodeScanned", new ScannedItem { Barcode = result, Quantity = quantity });

                        // Volver a la página anterior
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error", "Cantidad no válida.", "OK");
                        isScanning = true; // reactivar escaneo si la cantidad es inválida
                    }
                });
            }
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
