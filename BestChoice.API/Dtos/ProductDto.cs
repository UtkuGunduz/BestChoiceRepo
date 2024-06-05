namespace BestChoice.API.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal Rating { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string AdditionalFeatures { get; set; }
        public bool IsActive { get; set; }
        public string ReturnPolicy { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? DiscountId { get; set; }
        public string? DiscountName { get; set; }
        public decimal? DiscountAmount { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public int? StockId { get; set; }
        public int? StockQuantity { get; set; }
        public List<string> VisualUrls { get; set; }
    }
}
