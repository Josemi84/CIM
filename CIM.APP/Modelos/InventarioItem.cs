using SQLite;

namespace CIM.APP.Modelos
{
    public class InventarioItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public int Cantidad { get; set; }
        public int InventarioId { get; set; }
    }
}
