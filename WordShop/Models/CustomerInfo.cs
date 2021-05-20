using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WordShop.Enums;

namespace WordShop.Models
{
    public class CustomerInfo
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Заполните поле Имя")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Поле должно быть более {2}-х и меньше {1}-ти символов")]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Заполните поле Почта")]
        [StringLength(120, MinimumLength = 7, ErrorMessage = "Поле должно быть более {2}-ти и меньше {1}-ми символов")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", 
            ErrorMessage = "Адрес должен быть вида name@domain.com")]
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? PaymentDate { get; set; }

        public Courses Courses { get; set; } = WordShop.Enums.Courses.WordShop;
        public Level CourseLevel { get; set; } = WordShop.Enums.Level.Beginner;
        public PaymentStatus PaymentStatus { get; set; } = WordShop.Enums.PaymentStatus.Started;

        
        public int TariffId { get; set; }
        public virtual Tariff Tariff { get; set; }
    }
}