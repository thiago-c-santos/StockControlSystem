using ConsoleApp1.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace ConsoleApp1.Data
{
    public class ProductRepository
    {
        private readonly string _connectionString = "Data Source=stock.db";

        public void InsertProduct(Product product)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var insertCmd = connection.CreateCommand();

            //Converts the value to avoid error due to different cultures.
            string value = product.Price.ToString(CultureInfo.InvariantCulture);

            insertCmd.CommandText =
            $@"
            INSERT INTO Products (Name, Description, Price, StockAmount, Deleted)
            VALUES ('{product.Name}', '{product.Description}', {value}, {product.StockAmount}, false);
            ";
            insertCmd.ExecuteNonQuery();
            connection.Close();
        }

        public List<Product> GetAll()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "Select * from Products where Deleted = false";

            using var reader = selectCmd.ExecuteReader();

            List<Product> products = new List<Product>();           

            while (reader.Read())
            {
                if (!reader.HasRows)
                {
                    return products;
                }

                products.Add(new Product()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Price = reader.GetDouble(3),
                    StockAmount = reader.GetInt32(4),
                    Deleted = reader.GetBoolean(5)
                });
            }

            connection.Close();

            return products;
        }

        public List<Product> GetByName(string name)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = $"Select * from Products where Name like '%{name}%' and Deleted = false";

            using var reader = selectCmd.ExecuteReader();

            List<Product> products = new List<Product>();

            while (reader.Read())
            {
                if (!reader.HasRows)
                {
                    return products;
                }

                products.Add(new Product()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Price = reader.GetDouble(3),
                    StockAmount = reader.GetInt32(4),
                    Deleted = reader.GetBoolean(5)
                });
            }

            connection.Close();

            return products;
        }

        public void UpdateProduct(Product product)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var selectCmd = connection.CreateCommand();

            //Converts the value to avoid error due to different cultures.
            string value = product.Price.ToString(CultureInfo.InvariantCulture);

            selectCmd.CommandText =
                $@"
                    UPDATE Products
                    SET Name = '{product.Name}', Description = '{product.Description}', Price = {value}, StockAmount = {product.StockAmount}, Deleted = {product.Deleted}
                    WHERE Id = {product.Id}
                ;";

            selectCmd.ExecuteNonQuery();
            connection.Close();
        }

        public void DeleteProduct(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var selectCmd = connection.CreateCommand();
            selectCmd.CommandText =
                $@"
                    UPDATE Products
                    SET Deleted = true
                    WHERE Id = {id}
                ;";

            selectCmd.ExecuteNonQuery();
            connection.Close();
        }

        public void UpdateProductStock(int stockToDeduce, int productId)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var selectCmd = connection.CreateCommand();
            selectCmd.CommandText =
                $@"
                    UPDATE Products
                    SET StockAmount = StockAmount - {stockToDeduce}
                    WHERE Id = {productId}
                ;";

            selectCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
