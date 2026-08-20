using Npgsql;

namespace StoreConsoleApp
{
    public class SaleItemsService
    {
        private readonly string _connectionString;

        public SaleItemsService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ShowAllSaleItems()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
                SELECT si.id, si.saleid, si.productid, p.name, si.quantity, si.totalprice
                FROM saleitems si
                JOIN products p ON p.id = si.productid
                ORDER BY si.id", conn);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine("---- Sale Items ----");
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var saleId = reader.GetInt32(1);
                var productId = reader.GetInt32(2);
                var productName = reader.GetString(3);
                var quantity = reader.GetInt32(4);
                var totalPrice = reader.GetDecimal(5);

                Console.WriteLine($"[{id}] Sale #{saleId} — {productName} (#{productId}) — Qty: {quantity} — {totalPrice:0.00}");
            }
        }

        public void AddNewSaleItem(int saleId, int productId, int quantity, decimal totalPrice)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
                INSERT INTO saleitems (saleid, productid, quantity, totalprice)
                VALUES (@sale, @product, @qty, @total)", conn);
            cmd.Parameters.AddWithValue("sale", saleId);
            cmd.Parameters.AddWithValue("product", productId);
            cmd.Parameters.AddWithValue("qty", quantity);
            cmd.Parameters.AddWithValue("total", totalPrice);

            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("Sale item added.");
            }
            catch (PostgresException ex) when (ex.SqlState == "23503")
            {
                Console.WriteLine("Invalid sale_id or product_id — check that both exist.");
            }
        }

        public void UpdateQuantityOfSaleItem(int id, int quantity)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "UPDATE saleitems SET quantity = @qty WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("qty", quantity);
            cmd.Parameters.AddWithValue("id", id);

            var rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Sale item updated." : "Sale item not found.");
        }

        public void DeleteSaleItem(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM saleitems WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);

            var rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Sale item deleted." : "Sale item not found.");
        }
    }
}