using BestChoice.API.Models;

namespace BestChoice.API.Dtos
{
    public class UpdateCartItemDetailDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public int? ProductStockQuantity { get; set; }
        public string ProductSKU { get; set; }
        public Product ProductBrand { get; set; }
        public Category ProductCategory { get; set; }
        public List<string> ProductImageUrls { get; set; }
        public string ProductDescription { get; set; }
    }
}
