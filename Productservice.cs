using Npgsql;

namespace StoreConsoleApp
{
    public class ProductsService
    {
        private readonly string _connectionString;

        public ProductsService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ShowAllProducts()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "SELECT id, name, description, price, weight, categoryid FROM products ORDER BY id", conn);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine("---- Products ----");
            while (reader.Read())
            {
                var product = new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Price = reader.GetDecimal(3),
                    Weight = reader.IsDBNull(4) ? null : reader.GetDecimal(4),
                    CategoryId = reader.IsDBNull(5) ? null : reader.GetInt32(5)
                };
                Console.WriteLine(product);
            }
        }

        public void AddNewProduct(string name, string description, decimal price, decimal? weight, int? categoryId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
                INSERT INTO products (name, description, price, weight, categoryid)
                VALUES (@name, @desc, @price, @weight, @cat)", conn);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("desc", (object)description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("price", price);
            cmd.Parameters.AddWithValue("weight", (object)weight ?? DBNull.Value);
            cmd.Parameters.AddWithValue("cat", (object)categoryId ?? DBNull.Value);

            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("Product added.");
            }
            catch (PostgresException ex) when (ex.SqlState == "23503")
            {
                Console.WriteLine("Invalid category id — it does not exist.");
            }
        }

        public void UpdatePriceOfProduct(int id, decimal price)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "UPDATE products SET price = @price WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("price", price);
            cmd.Parameters.AddWithValue("id", id);

            var rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Product updated." : "Product not found.");
        }

        public void DeleteProduct(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM products WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);

            try
            {
                var rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Product deleted." : "Product not found.");
            }
            catch (PostgresException ex) when (ex.SqlState == "23503")
            {
                Console.WriteLine("Cannot delete: this product is referenced in stocks or sale items.");
            }
        }
    }
}