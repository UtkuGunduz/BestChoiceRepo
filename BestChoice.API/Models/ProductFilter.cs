namespace BestChoice.API.Models
{
    public class ProductFilter
    {
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public int? CategoryId { get; set; }
        public int? DiscountId { get; set; }
    }
}