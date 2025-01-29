namespace ConsoleApp1.Models
{
    public class ProductHistory
    {
        public int Id { get; set; } 

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime DateAdded { get; set; }
    }
}
