namespace BestChoice.API.Dtos
{
    public class CartSummaryDto
    {
        public int ItemCount { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountedTotalPrice { get; set; }
    }
}
