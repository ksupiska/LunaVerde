namespace LunaVerde.Models
{
    public class CartItem
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
