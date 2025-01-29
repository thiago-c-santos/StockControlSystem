using ConsoleApp1.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp1.Data
{
    public class ProductRepository
    {
        private readonly string _connectionString = "Data Source=stock.db";

        #region DatabaseOperations

        public void InsertProduct(Product product)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var insertCmd = connection.CreateCommand();

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
            selectCmd.CommandText = "Select * from Products";

            using var reader = selectCmd.ExecuteReader();

            List<Product> products = new List<Product>();

            while (reader.Read())
            {
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
            selectCmd.CommandText = $"Select * from Products where Name like '%{name}%'";

            using var reader = selectCmd.ExecuteReader();

            List<Product> products = new List<Product>();

            while (reader.Read())
            {
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
            selectCmd.CommandText =
                $@"
                    UPDATE Products
                    SET Name = '{product.Name}', Description = '{product.Description}', Price = {product.Price}, StockAmount = {product.StockAmount}, Deleted = {product.Deleted}
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

        #endregion

        #region DatabaseStartupChecks

        public void CheckTableExists()
        {
            using var connection = new SqliteConnection(_connectionString);

            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS Products (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Description TEXT NULL,
                    Price REAL NOT NULL,
                    StockAmount INTEGER NOT NULL,
                    Deleted BOOL DEFAULT FALSE
            );";

            command.ExecuteNonQuery();

            command.CommandText =
                @"CREATE TABLE IF NOT EXISTS ProductsHistory(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Description TEXT NULL,
                    DateAdded TIMESTAMP NOT NULL
                );";

            command.ExecuteNonQuery();

            command.CommandText =
                @"CREATE TABLE IF NOT EXISTS Employee(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Cpf TEXT NOT NULL,
                    HiringDate TIMESTAMP NOT NULL,
                    UnemploymentDate TIMESTAMP NULL,
                    IsEmployed BOOL DEFAULT FALSE
                );";

            command.ExecuteNonQuery();

            command.CommandText =
                @"CREATE TABLE IF NOT EXISTS Sale (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProductId INTEGER NOT NULL,
                    EmployeeId INTEGER NOT NULL,
                    AmountSold INTEGER NOT NULL,
                    SaleTime TIMESTAMP NOT NULL,
                    FOREIGN KEY (ProductId) REFERENCES Products(Id),
                    FOREIGN KEY (EmployeeId) REFERENCES Employee(Id)
                );";
            command.ExecuteNonQuery();

            connection.Close();
        }

        #endregion
    }
}
