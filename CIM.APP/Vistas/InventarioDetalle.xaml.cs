using System.Collections.ObjectModel;
using CIM.APP.Modelos;
using Microsoft.Maui.Controls;

namespace CIM.APP
{
    [QueryProperty(nameof(InventarioId), "InventarioId")]
    public partial class InventarioDetalle : ContentPage
    {
        private int inventarioId;

        public int InventarioId
        {
            get => inventarioId;
            set
            {
                inventarioId = value;
                CargarProductos(inventarioId);
            }
        }

        public ObservableCollection<InventarioItem> Productos { get; set; } = new();

        public InventarioDetalle()
        {
            InitializeComponent();
            ProductosCollectionView.ItemsSource = Productos;
        }

        private async void CargarProductos(int id)
        {
            var items = await App.Database.GetItemsAsync(id);
            Productos.Clear();
            foreach (var item in items)
            {
                Productos.Add(item);
            }
        }
    }
}
