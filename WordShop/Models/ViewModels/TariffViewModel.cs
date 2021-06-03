using System.Collections.Generic;

namespace WordShop.Models.ViewModels
{
    public class TariffViewModel
    {
        public Tariff Tariff { get; set; }
        public List<TariffBenefit> TariffBenefits { get; set; }
    }
}