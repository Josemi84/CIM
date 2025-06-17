using System.Collections.Generic;
using CIM.APP.Modelos;
using Microsoft.Maui.Controls;

namespace CIM.APP
{
    public partial class InventariosGuardados : ContentPage
    {
        public InventariosGuardados()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var inventarios = await App.Database.GetInventariosAsync();
            InventariosCollectionView.ItemsSource = inventarios;
        }

        private async void OnInventarioSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
                return;

            var inventarioSeleccionado = e.CurrentSelection[0] as Inventario;
            if (inventarioSeleccionado == null)
                return;

            // Navegar a InventarioDetalle con el InventarioId
            await Shell.Current.GoToAsync($"InventarioDetalle?InventarioId={inventarioSeleccionado.Id}");

            // Deseleccionar el item después de la selección
            ((CollectionView)sender).SelectedItem = null;
        }

    }
}
