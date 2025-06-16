using SQLite;
using System;

namespace CIM.APP.Modelos
{
    public class Inventario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
