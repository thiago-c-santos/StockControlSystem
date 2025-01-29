namespace StockControl.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public virtual int ProductId { get; set; }

        public virtual int EmployeeId {  get; set; }

        public int AmountSold { get; set; }

        public DateTime SaleTime { get; set; }
    }
}
