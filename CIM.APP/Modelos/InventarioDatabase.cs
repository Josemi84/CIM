using SQLite;

namespace CIM.APP.Modelos
{
    public class InventarioDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public InventarioDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Inventario>().Wait();
            _database.CreateTableAsync<InventarioItem>().Wait();
        }

        public Task<List<Inventario>> GetInventariosAsync() => _database.Table<Inventario>().ToListAsync();
        public Task<int> SaveInventarioAsync(Inventario inventario) =>
            inventario.Id != 0 ? _database.UpdateAsync(inventario) : _database.InsertAsync(inventario);
        public Task<int> DeleteInventarioAsync(Inventario inventario) => _database.DeleteAsync(inventario);

        public Task<List<InventarioItem>> GetItemsAsync(int inventarioId) =>
            _database.Table<InventarioItem>().Where(i => i.InventarioId == inventarioId).ToListAsync();

        public Task<int> SaveItemAsync(InventarioItem item) =>
            item.Id != 0 ? _database.UpdateAsync(item) : _database.InsertAsync(item);
        public Task<int> DeleteItemAsync(InventarioItem item) => _database.DeleteAsync(item);
    }
}
