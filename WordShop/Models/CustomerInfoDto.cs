using WordShop.Enums;

namespace WordShop.Models
{
    public class CustomerInfoDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string TariffName { get; set; }
        public string TariffNewPrice { get; set; }
        public string Courses { get; set; }
        public string CreatedDate { get; set; }
        public string PaymentStatus { get; set; }
    }
}