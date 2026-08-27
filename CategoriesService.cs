using Dapper;
using Npgsql;

namespace StoreConsoleApp
{
    public class CategoriesService
    {
        private readonly string _connectionString;

        public CategoriesService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ShowAllCategories()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT id, name, description, parentcategoryid, isactive FROM categories ORDER BY id", conn);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine("---- Categories ----");
            while (reader.Read())
            {
                var category = new Category
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                    ParentCategoryId = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    IsActive = reader.GetBoolean(4)
                };
                Console.WriteLine(category);
            }
        }

        public void AddNewCategory(string name, string description, int? parentCategoryId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO categories (name, description, parentcategoryid) VALUES (@name, @desc, @parent)", conn);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("desc", (object)description);
            cmd.Parameters.AddWithValue("parent", (object)parentCategoryId);

            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("Category added.");
            }
            catch (PostgresException ex) when (ex.SqlState == "23503")
            {
                Console.WriteLine("Invalid parent category id — it does not exist.");
            }
        }

        public void UpdateDescriptionOfCategory(int id, string description)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "UPDATE categories SET description = @desc WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("desc", description);
            cmd.Parameters.AddWithValue("id", id);

            var rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Category updated." : "Category not found.");
        }

        public void DeleteCategory(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM categories WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);

            var rows = cmd.ExecuteNonQuery();
            Console.WriteLine(rows > 0 ? "Category deleted." : "Category not found.");
        }


         public void ShowAllCategorieswithdapper()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            var categories = conn.Query<Category>("select * from categories").ToList();
            foreach(var a in categories)
            {
                System.Console.WriteLine(a.Id + " " + a.Name + " " + a.Description);
            }
        }

        
        public void AddNewCategorywithdapper(string name, string description)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            var res = conn.Execute("insert into categories (name,description) values (@name , @description)", new{name,description});
            if (res > 0)
            {
                System.Console.WriteLine("add shid");
            }
            else
            {
                System.Console.WriteLine("nashid");
            }
        }

              public void UpdateDescriptionOfCategorywithdapper(int id, string description)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            var res = conn.Execute("update categories set description = @description where id = @id", new{id,description});
              if (res > 0)
            {
                System.Console.WriteLine("update shid");
            }
            else
            {
                System.Console.WriteLine("nashid");
            }
        }

        public void Deletecategorywithdapper(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            var res = conn.Execute("delete from categories where id = @id" , new{id});
              if (res > 0)
            {
                System.Console.WriteLine("ud shid");
            }
            else
            {
                System.Console.WriteLine("nashid");
            }
        }
    }
}