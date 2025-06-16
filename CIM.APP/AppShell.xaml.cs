namespace CIM.APP
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(InventariosGuardados), typeof(InventariosGuardados));
            Routing.RegisterRoute(nameof(InventarioDetalle), typeof(InventarioDetalle));
        }
    }
}
