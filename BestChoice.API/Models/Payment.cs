namespace BestChoice.API.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } // Navigation property
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}