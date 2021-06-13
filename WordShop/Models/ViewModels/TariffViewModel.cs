using System.Collections.Generic;
using WordShop.Models.Tariff;

namespace WordShop.Models.ViewModels
{
    public class TariffViewModel
    {
        public Tariff.Tariff Tariff { get; set; }
        public List<TariffBenefit> TariffBenefits { get; set; }
    }
}