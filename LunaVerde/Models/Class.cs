namespace LunaVerde.Models
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int MenuId { get; set; }
        public int Quantity { get; set; }

        public Order Order { get; set; }
        public Menu Menu { get; set; }
    }

    public class Customer
    {
        public int CustomerID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Связь с таблицей Reservations
        public ICollection<Reservation> Reservations { get; set; }
    }
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int CustomerID { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan ReservationTime { get; set; }
        public int TableNumber { get; set; }
        public int GuestsCount { get; set; }
        public string SpecialRequests { get; set; }

        // Связь с таблицей Customers
        public Customer Customer { get; set; }
    }

}
