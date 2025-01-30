using Microsoft.Data.Sqlite;
using StockControl.Models;

namespace StockControl.Data
{
    public class SaleRepository
    {
        private readonly string _connectionString = "Data Source=stock.db";

        public void AddSale(Sale sale)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var insertCmd = connection.CreateCommand();

            string saleTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ss");

            insertCmd.CommandText =
            $@"
                INSERT INTO Sale (ProductId, EmployeeId, AmountSold, SaleTime)
                VALUES ({sale.ProductId}, {sale.EmployeeId}, {sale.AmountSold}, '{saleTime}');
            ";

            insertCmd.ExecuteNonQuery();
            connection.Close();
        }

        public List<SalesDTO> ListSales(int? productId)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var selectCmd = connection.CreateCommand();

            selectCmd.CommandText =
            $@"
                Select Employee.Name, Products.Name, count(AmountSold) FROM Sale
                INNER JOIN Products on Products.Id = Sale.ProductId
                INNER JOIN Employee on Employee.Id = Sale.EmployeeId
            ";

            using var reader = selectCmd.ExecuteReader();

            List<SalesDTO> sales = new List<SalesDTO>();

            while (reader.Read())
            {
                if (!reader.HasRows)
                {
                    return sales;
                }

                sales.Add(new SalesDTO()
                {
                    EmployeeName = reader.IsDBNull(0) ? null :reader.GetString(0),
                    ProductName = reader.IsDBNull(1) ? null : reader.GetString(1),
                    SoldAmount = reader.IsDBNull(2) ? null : reader.GetInt32(2)
                });
            }

            connection.Close();

            return sales;
        }
    }
}
