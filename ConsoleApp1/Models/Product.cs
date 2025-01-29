﻿namespace ConsoleApp1.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public double Price { get; set; }

        public int StockAmount { get; set; }

        public bool Deleted { get; set; }
    }
}
