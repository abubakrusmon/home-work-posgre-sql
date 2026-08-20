using Npgsql;

namespace StoreConsoleApp
{
    public class StocksService
    {
        private readonly string _connectionString;

        public StocksService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ShowAllStocks()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            // Джойним с products/warehouses, чтобы сразу видеть названия
            using var cmd = new NpgsqlCommand(@"
                SELECT s.id, s.productid, p.name, s.warehouseid, w.name, s.quantity
                FROM stocks s
                JOIN products p ON p.id = s.productid
                JOIN warehouses w ON w.id = s.warehouseid
                ORDER BY s.id", conn);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine("---- Stocks ----");
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var productId = reader.GetInt32(1);
                var productName = reader.GetString(2);
                var warehouseId = reader.GetInt32(3);
                var warehouseName = reader.GetString(4);
                var quantity = reader.GetInt32(5);

                Console.WriteLine($"[{id}] {productName} (#{productId}) @ {warehouseName} (#{warehouseId}) — Qty: {quantity}");
            }
        }

        public void AddNewStock(int productId, int warehouseId, int quantity)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
                INSERT INTO stocks (productid, warehouseid, quantity)
                VALUES (@p, @w, @q)", conn);
            cmd.Parameters.AddWithValue("p", productId);
            cmd.Parameters.AddWithValue("w", warehouseId);
            cmd.Parameters.AddWithValue("q", quantity);

            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("Stock record added.");
            }
            catch (PostgresException ex) when (ex.SqlState == "23503")
            {
                Console.WriteLine("Invalid product_id or warehouse_id — check that both exist.");
            }
            catch (PostgresException ex) when (ex.SqlState == "23505")
            {
                Console.WriteLine("This product already has a stock record for that warehouse — update it instead.");
            }
        }

        public void UpdateQuantityOfStock(int id, int quantity)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "UPDATE stocks SET quantity = @q WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("q", quantity);
            cmd.Parameters.AddWithValue("id", id);

            var rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Stock updated." : "Stock record not found.");
        }

        public void DeleteStock(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM stocks WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);

            var rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Stock record deleted." : "Stock record not found.");
        }
    }
}