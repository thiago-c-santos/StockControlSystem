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

        public void RemoveEmployee(int id)
        {
            _repository.RemoveEmployee(id);
        }

        public List<Employee> GetAllExEmployees()
        {
            return _repository.GetAllExEmployees();
        }
    }
}
