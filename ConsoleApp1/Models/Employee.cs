namespace StockControl.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Cpf { get; set; } = string.Empty;

        public DateTime HiringDate { get; set; }

        public DateTime UnemploymentDate { get; set; }

        public bool IsEmployed { get; set; }
    }
}
