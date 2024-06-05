namespace BestChoice.API.Dtos
{
    public class CheckoutDto
    {
        public string ShippingAddress { get; set; } // Customer's shipping address
        public string PaymentInformation { get; set; } // Encrypted payment information (e.g., token)
    }
}
