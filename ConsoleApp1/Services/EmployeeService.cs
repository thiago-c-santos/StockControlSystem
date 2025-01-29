using StockControl.Data;
using StockControl.Models;

namespace StockControl.Services
{
    public class EmployeeService
    {
        private readonly EmployeeRepository _repository = new EmployeeRepository();

        public void AddEmployee(Employee employee)
        {
            _repository.AddEmployee(employee);
        }

        public Employee GetEmployeeByCpf(string cpf)
        {
            return _repository.GetEmployeeByCpf(cpf);   
        }

        public List<Employee> GetAllEmployees()
        {
            return _repository.GetAllEmployees();
        }
    }
}
