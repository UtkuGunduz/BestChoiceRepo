namespace BestChoice.API.Models
{
    public class Product
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
        public Category Category { get; set; }
        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }
        public int StockQuantity { get; set; }
        public List<Visual> Visuals { get; set; }
        public List<Review> Reviews { get; set; }
        public List<WishlistItem>? WishlistItems { get; set; }
    }
}