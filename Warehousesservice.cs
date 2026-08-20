using Npgsql;

namespace StoreConsoleApp
{
    public class WarehousesService
    {
        private readonly string _connectionString;

        public WarehousesService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ShowAllWarehouses()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "SELECT id, name, address, isactive FROM warehouses ORDER BY id", conn);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine("---- Warehouses ----");
            while (reader.Read())
            {
                var warehouse = new Warehouse
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Address = reader.IsDBNull(2) ? null : reader.GetString(2),
                    IsActive = reader.GetBoolean(3)
                };
                Console.WriteLine(warehouse);
            }
        }

        public void AddNewWarehouse(string name, string address)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "INSERT INTO warehouses (name, address) VALUES (@name, @address)", conn);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("address", (object)address ?? DBNull.Value);
            cmd.ExecuteNonQuery();

            Console.WriteLine("Warehouse added.");
        }

        public void UpdateAddressOfWarehouse(int id, string address)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "UPDATE warehouses SET address = @address WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("address", address);
            cmd.Parameters.AddWithValue("id", id);

            var rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Warehouse updated." : "Warehouse not found.");
        }

        public void DeleteWarehouse(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM warehouses WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);

            try
            {
                var rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Warehouse deleted." : "Warehouse not found.");
            }
            catch (PostgresException ex) when (ex.SqlState == "23503")
            {
                Console.WriteLine("Cannot delete: this warehouse has sales linked to it (sales reference warehouses with RESTRICT).");
            }
        }
    }
}