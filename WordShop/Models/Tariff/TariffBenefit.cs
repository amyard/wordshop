using System.Collections.Generic;

namespace WordShop.Models.Tariff
{
    public class TariffBenefit : BaseEntity
    {
        public string Benefit { get; set; }
        public ICollection<TariffBenefitOrdered> TariffBenefitOrdered { get; set; }
    }
}