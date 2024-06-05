namespace BestChoice.API.Models
{
    public class Visual
    {
        public int Id { get; set; }
        public string? PhotoUrl { get; set; }
        public string? VideoUrl { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
