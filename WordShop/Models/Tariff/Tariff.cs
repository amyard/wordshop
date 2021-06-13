using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WordShop.Enums;

namespace WordShop.Models.Tariff
{
    public class Tariff : BaseEntity
    {
        [Display(Name= "Hазвание тарифа")]
        public string Name { get; set; }
        
        [Display(Name= "Старая цена")]
        public int OldPrice { get; set; }
        
        [Display(Name= "Новая цена")]
        public int NewPrice { get; set; }
        
        public Courses Courses { get; set; } = Courses.WordShop;
        public Level Level { get; set; } = Level.Beginner;
        
        public List<CustomerInfo.CustomerInfo> CustomerInfos { get; set; }

        [Display(Name= "Преимущества")]
        public IList<TariffBenefitOrdered> Advantage { get; set; }
        
        [Display(Name= "Недостатки")]
        public IList<TariffBenefitOrdered> Disadvantage { get; set; }
    }
}