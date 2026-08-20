using Npgsql;

namespace StoreConsoleApp
{
    public class SalesService
    {
        private readonly string _connectionString;

        public SalesService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ShowAllSales()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
                SELECT s.id, s.warehouseid, w.name, s.customerid, c.fullname,
                       s.saledate, s.status, s.totalprice, s.discountprice
                FROM sales s
                JOIN warehouses w ON w.id = s.warehouseid
                JOIN customers c ON c.id = s.customerid
                ORDER BY s.id", conn);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine("---- Sales ----");
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var warehouseId = reader.GetInt32(1);
                var warehouseName = reader.GetString(2);
                var customerId = reader.GetInt32(3);
                var customerName = reader.GetString(4);
                var saleDate = reader.GetDateTime(5);
                var status = reader.GetString(6);
                var totalPrice = reader.GetDecimal(7);
                var discountPrice = reader.GetDecimal(8);

                Console.WriteLine(
                    $"[{id}] {saleDate:yyyy-MM-dd} — {warehouseName} (#{warehouseId}) — {customerName} (#{customerId}) — {status} — Total: {totalPrice:0.00} (discount {discountPrice:0.00})");
            }
        }

        public void AddNewSale(int warehouseId, int customerId, DateTime saleDate, string status, decimal totalPrice, decimal discountPrice)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
                INSERT INTO sales (warehouseid, customerid, saledate, status, totalprice, discountprice)
                VALUES (@w, @c, @date, @status, @total, @discount)", conn);
            cmd.Parameters.AddWithValue("w", warehouseId);
            cmd.Parameters.AddWithValue("c", customerId);
            cmd.Parameters.AddWithValue("date", saleDate);
            cmd.Parameters.AddWithValue("status", status);
            cmd.Parameters.AddWithValue("total", totalPrice);
            cmd.Parameters.AddWithValue("discount", discountPrice);

            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("Sale added.");
            }
            catch (PostgresException ex) when (ex.SqlState == "23503")
            {
                Console.WriteLine("Invalid warehouse_id or customer_id — check that both exist.");
            }
        }

        public void UpdateStatusOfSale(int id, string status)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "UPDATE sales SET status = @status WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("status", status);
            cmd.Parameters.AddWithValue("id", id);

            var rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Sale updated." : "Sale not found.");
        }

        public void DeleteSale(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            // SaleItems ссылается на Sales с ON DELETE CASCADE, поэтому связанные позиции удалятся автоматически
            using var cmd = new NpgsqlCommand("DELETE FROM sales WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);

            var rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Sale deleted (its sale items were removed too)." : "Sale not found.");
        }
    }
}