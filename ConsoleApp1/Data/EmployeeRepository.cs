using ConsoleApp1.Models;
using Microsoft.Data.Sqlite;
using StockControl.Models;
using System.Globalization;
using System.Xml.Linq;

namespace StockControl.Data
{
    public class EmployeeRepository
    {
        private readonly string _connectionString = "Data Source=stock.db";

        public void AddEmployee(Employee employee)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var insertCmd = connection.CreateCommand();

            string hiringDate = employee.HiringDate.ToString("yyyy-MM-dd HH:mm:ss.ss");

            insertCmd.CommandText =
            $@"
                INSERT INTO Employee(Name, Cpf, HiringDate, UnemploymentDate, IsEmployed)
                VALUES ('{employee.Name}', '{employee.Cpf}', '{hiringDate}', NULL, true)
            ;";
            insertCmd.ExecuteNonQuery();
            connection.Close();
        }

        public Employee GetEmployeeByCpf(string cpf)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = $"Select * from Employee where Cpf = '{cpf}' and IsUnemployed = false";

            using var reader = selectCmd.ExecuteReader();            

            Employee employee = new Employee();

            while (reader.Read())
            {
                employee.Id = reader.GetInt32(0);
                employee.Name = reader.GetString(1);
                employee.Cpf = reader.GetString(2);
                employee.HiringDate = Convert.ToDateTime(reader.GetTimeSpan(3));
                employee.UnemploymentDate = Convert.ToDateTime(reader.GetTimeSpan(4));
                employee.IsEmployed = reader.GetBoolean(5);
            }

            connection.Close();

            return employee;
        }

        public List<Employee> GetAllEmployees()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = $"Select * from Employee where IsEmployed = true";

            using var reader = selectCmd.ExecuteReader();

            List<Employee> employees = new List<Employee>();

            while (reader.Read())
            {
                employees.Add(new Employee()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Cpf = reader.GetString(2),
                    HiringDate = reader.GetDateTime(3),
                    UnemploymentDate = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4),
                    IsEmployed = reader.GetBoolean(5),
                });
            }

            connection.Close();

            return employees;
        }
    }
}
