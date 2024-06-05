namespace BestChoice.API.Dtos
{
    public class ProductDetailsDto
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
        public string Brand { get; set; }

        public int CategoryId { get; set; }
        public CategoryDto Category { get; set; } // Use CategoryDto instead of Category

        public int? DiscountId { get; set; }
        public DiscountDto Discount { get; set; } // Use DiscountDto instead of Discount

        public int StockQuantity { get; set; }
        public List<VisualDto> Visuals { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }
}