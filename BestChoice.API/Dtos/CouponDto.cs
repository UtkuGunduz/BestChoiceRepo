namespace BestChoice.API.Dtos
{
    public class CouponDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double DiscountPercentage { get; set; }
        public bool IsValid { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
