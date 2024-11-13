namespace ECommerce.Core.DTOs
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }

    public class OrderItemCreateDTO
    {
        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; }
        public int ProductId { get; set; }
    }
    public class OrderItemUpdateDTO : OrderItemCreateDTO { }
}
