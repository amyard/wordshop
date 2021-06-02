using System.Collections.Generic;
using WordShop.Enums;

namespace WordShop.Models
{
    public class Tariff : BaseEntity
    {
        public string Name { get; set; }
        public int OldPrice { get; set; }
        public int NewPrice { get; set; }
        
        public Courses Courses { get; set; } = Courses.WordShop;
        public Level Level { get; set; } = Level.Beginner;
        
        public List<CustomerInfo> CustomerInfos { get; set; }

        public ICollection<TariffBenefitOrdered> Advantage { get; set; }
        public ICollection<TariffBenefitOrdered> Disadvantage { get; set; }
    }
}