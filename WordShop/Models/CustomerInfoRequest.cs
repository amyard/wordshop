namespace WordShop.Models
{
    public class CustomerInfoRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int TariffId { get; set; }
    }
}