namespace BestChoice.API.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public string? Comment { get; set; }
        public decimal? Rating { get; set; }
        public DateTime Created { get; set; }
    }
}
