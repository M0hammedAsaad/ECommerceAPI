namespace ECommerce.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageURL { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CategoryID { get; set; }
        public Category Category { get; set; }
    }
}
