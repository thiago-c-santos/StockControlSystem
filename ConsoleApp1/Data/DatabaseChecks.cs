using Microsoft.Data.Sqlite;

namespace StockControl.Data
{
    public class DatabaseChecks
    {
        private readonly string _connectionString = "Data Source=stock.db";

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
