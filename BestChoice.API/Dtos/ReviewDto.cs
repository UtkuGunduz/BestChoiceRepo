namespace BestChoice.API.Dtos
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public decimal? Rating { get; set; }
        public DateTime Created { get; set; }
    }
}
