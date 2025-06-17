using CIM.APP.Modelos;

namespace CIM.APP
{
    public partial class App : Application
    {
        private static InventarioDatabase database;

        public static InventarioDatabase Database
        {
            get
            {
                if (database == null)
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "inventarios.db3");
                    database = new InventarioDatabase(dbPath);
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
