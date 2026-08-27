using Npgsql;

namespace StoreConsoleApp
{
    public class CustomersService
    {
        private readonly string _connectionString;

        public CustomersService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ShowAllCustomers()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "SELECT id, fullname, email, phonnumber, address FROM customers ORDER BY id", conn);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine("---- Customers ----");
            while (reader.Read())
            {
                var customer = new Customer
                {
                    Id = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Email = reader.IsDBNull(2) ? null : reader.GetString(2),
                    PhoneNumber = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Address = reader.IsDBNull(4) ? null : reader.GetString(4)
                };
                Console.WriteLine(customer);
            }
        }

        public void AddNewCustomer(string fullName, string email, string phoneNumber, string address)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(@"
                INSERT INTO customers (fullname, email, phonnumber, address)
                VALUES (@fullname, @email, @phone, @address)", conn);
            cmd.Parameters.AddWithValue("fullname", fullName);
            cmd.Parameters.AddWithValue("email", (object)email );
            cmd.Parameters.AddWithValue("phone", (object)phoneNumber);
            cmd.Parameters.AddWithValue("address", (object)address);
            cmd.ExecuteNonQuery();

            Console.WriteLine("Customer added.");
        }

        public void UpdatePhoneOfCustomer(int id, string phoneNumber)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "UPDATE customers SET phonnumber = @phone WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("phone", phoneNumber);
            cmd.Parameters.AddWithValue("id", id);

            var rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Customer updated." : "Customer not found.");
        }

        public void DeleteCustomer(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM customers WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);

            try
            {
                var rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Customer deleted." : "Customer not found.");
            }
            catch (PostgresException ex) when (ex.SqlState == "23503")
            {
                Console.WriteLine("Cannot delete: this customer has sales linked to them (sales reference customers with RESTRICT).");
            }
        }
    }
}