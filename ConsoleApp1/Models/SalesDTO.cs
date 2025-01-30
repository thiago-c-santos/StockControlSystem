namespace StockControl.Models
{
    public class SalesDTO
    {
        public string ProductName { get; set; } = string.Empty;

        public int? SoldAmount { get; set; }

        public string? EmployeeName { get; set; } = string.Empty;
    }
}
