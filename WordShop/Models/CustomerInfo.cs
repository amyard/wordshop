using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WordShop.Enums;

namespace WordShop.Models
{
    public class CustomerInfo
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Заполните поле")]
        [StringLength(80, ErrorMessage = "Имя должно быть более 3-х символов")]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Заполните поле")]
        [StringLength(120, ErrorMessage = "Поле должно быть более 5-ти символов")]
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }

        public Courses Courses { get; set; } = WordShop.Enums.Courses.WordShop;
        public Level CourseLevel { get; set; } = WordShop.Enums.Level.Beginner;
        public PaymentStatus PaymentStatus { get; set; } = WordShop.Enums.PaymentStatus.Started;

        
        public int TariffId { get; set; }
        public Tariff Tariff { get; set; }
    }
}