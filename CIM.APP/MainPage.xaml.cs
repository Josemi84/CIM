using CIM.APP.Modelos;
using System.Collections.ObjectModel;
using System.Text;

namespace CIM.APP
{
    public partial class MainPage : ContentPage
    {
        private Inventario inventarioActivo;
        public ObservableCollection<InventarioItem> Items { get; set; }

        public MainPage()
        {
            InitializeComponent();

            inventarioActivo = new Inventario
            {
                Nombre = "Inventario temporal",
                FechaCreacion = DateTime.Now
            };

            Items = new ObservableCollection<InventarioItem>();
            ProductsCollectionView.ItemsSource = Items;

            // Suscribirse al mensaje de código escaneado
            MessagingCenter.Subscribe<BarcodeScannerPage, ScannedItem>(this, "BarcodeScanned", (sender, scannedItem) =>
            {
                BarcodeScanned(scannedItem);
            });
        }

        private void BarcodeScanned(ScannedItem item)
        {
            // Actualiza el Label con el código escaneado
            ScannedBarcodeLabel.Text = $"Código escaneado: {item.Barcode}";
            // Establece la cantidad por defecto (puedes dejarlo vacío o en 1)
            QuantityEntry.Text = item.Quantity.ToString();
        }

        // Este método abre la página de escaneo cuando se hace clic en el botón
        private async void OnScanClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BarcodeScannerPage());
        }

        private async void SendEmailClicked(object sender, EventArgs e)
        {
            try
            {
                string fileName = $"inventario_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                if (!File.Exists(filePath))
                {
                    await DisplayAlert("Error", "El archivo CSV no se pudo generar.", "OK");
                    return;
                }

                var emailMessage = new EmailMessage
                {
                    Subject = "Inventario CSV",
                    Body = "Adjunto el archivo con el inventario en formato CSV.",
                    To = new List<string> { "example@domain.com" }
                };

                emailMessage.Attachments.Add(new EmailAttachment(filePath));

                await Email.Default.ComposeAsync(emailMessage);

                await DisplayAlert("Éxito", "El inventario ha sido enviado por correo.", "OK");
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Error", "El envío de correos electrónicos no está soportado en este dispositivo.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Hubo un problema al enviar el correo: {ex.Message}", "OK");
            }
        }

        private async void ExportCsvClicked(object sender, EventArgs e)
        {
            try
            {
                var csv = new StringBuilder();
                csv.AppendLine("Codigo,Cantidad");

                foreach (var item in Items)
                {
                    csv.AppendLine($"{item.Codigo},{item.Cantidad}");
                }

                string fileName = $"inventario_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                File.WriteAllText(filePath, csv.ToString());

                await DisplayAlert("Éxito", $"Inventario exportado a CSV en:\n{filePath}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo exportar el CSV: {ex.Message}", "OK");
            }
        }

        private void AddItemClicked(object sender, EventArgs e)
        {
            string codigo = ScannedBarcodeLabel.Text?.Replace("Código escaneado: ", "").Trim();
            bool qtyParsed = int.TryParse(QuantityEntry.Text, out int cantidad);

            if (string.IsNullOrEmpty(codigo) || codigo == "-")
            {
                DisplayAlert("Error", "Escanea un código de barras primero.", "OK");
                return;
            }

            if (!qtyParsed || cantidad <= 0)
            {
                DisplayAlert("Error", "Introduce una cantidad válida mayor que 0.", "OK");
                return;
            }

            var existingItem = Items.FirstOrDefault(i => i.Codigo == codigo);

            if (existingItem != null)
            {
                existingItem.Cantidad += cantidad;
            }
            else
            {
                var newItem = new InventarioItem
                {
                    Codigo = codigo,
                    Cantidad = cantidad
                };
                Items.Add(newItem);
            }

            ScannedBarcodeLabel.Text = "Código escaneado: -";
            QuantityEntry.Text = string.Empty;
        }

        private async void SaveInventoryClicked(object sender, EventArgs e)
        {
            string nombreInventario = await DisplayPromptAsync("Guardar Inventario", "Introduce un nombre para el inventario:");

            if (string.IsNullOrWhiteSpace(nombreInventario))
                return;

            inventarioActivo.Nombre = nombreInventario;
            inventarioActivo.FechaCreacion = DateTime.Now;

            await App.Database.SaveInventarioAsync(inventarioActivo);

            foreach (var item in Items)
            {
                item.InventarioId = inventarioActivo.Id;
                await App.Database.SaveItemAsync(item);
            }

            await DisplayAlert("Éxito", "Inventario guardado con éxito.", "OK");

            Items.Clear();
            inventarioActivo = new Inventario
            {
                Nombre = "Inventario temporal",
                FechaCreacion = DateTime.Now
            };
        }

        private async void ViewInventariosClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("InventariosGuardados");
        }
    }
}
