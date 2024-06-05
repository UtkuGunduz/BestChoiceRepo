namespace BestChoice.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; } // Added OrderStatus
        public decimal TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public Payment Payment { get; set; } // Added Payment
    }
}