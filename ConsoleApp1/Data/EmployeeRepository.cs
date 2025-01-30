using Microsoft.Data.Sqlite;
using StockControl.Models;

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
            selectCmd.CommandText = $"Select * from Employee where Cpf = '{cpf}' and IsEmployed = true";

            using var reader = selectCmd.ExecuteReader();            

            Employee employee = new Employee();

            while (reader.Read())
            {
                if (!reader.HasRows)
                {
                    return employee;
                }

                employee.Id = reader.GetInt32(0);
                employee.Name = reader.GetString(1);
                employee.Cpf = reader.GetString(2);
                employee.HiringDate = reader.GetDateTime(3);
                employee.UnemploymentDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
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
                if (!reader.HasRows)
                {
                    return employees;
                }

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

        public void RemoveEmployee(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var updateCmd = connection.CreateCommand();

            string unemploymentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ss");

            updateCmd.CommandText = $"Update Employee Set IsEmployed = false, UnemploymentDate = '{unemploymentDate}' where Id = {id}";

            updateCmd.ExecuteNonQuery();

            connection.Close();
        }

        public List<Employee> GetAllExEmployees()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = $"Select * from Employee where IsEmployed = false";

            using var reader = selectCmd.ExecuteReader();

            List<Employee> employees = new List<Employee>();

            while (reader.Read())
            {
                if (!reader.HasRows)
                {
                    return employees;
                }

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
