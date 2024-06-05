namespace BestChoice.API.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double DiscountPercentage { get; set; }
        public bool IsValid { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}