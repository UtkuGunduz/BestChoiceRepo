namespace BestChoice.API.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
