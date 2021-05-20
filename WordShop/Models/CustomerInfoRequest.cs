using System.ComponentModel.DataAnnotations;

namespace WordShop.Models
{
    public class CustomerInfoRequest
    {
        [Required(ErrorMessage = "Заполните поле Имя")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Поле должно быть более {2}-ти и меньше {1}-ти символов")]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Заполните поле Почта")]
        [StringLength(120, MinimumLength = 7, ErrorMessage = "Поле должно быть более {2}-ти и меньше {1}-ми символов")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", 
            ErrorMessage = "Адрес должен быть вида name@domain.com")]
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Выберите тариф")]
        public int TariffId { get; set; }
    }
}