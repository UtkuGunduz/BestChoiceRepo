namespace BestChoice.API.Dtos
{
    public class WishlistItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public DateTime AddedAt { get; set; }
        public ProductDto? Product { get; set; }
    }
}
