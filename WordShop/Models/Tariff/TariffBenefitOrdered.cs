namespace WordShop.Models.Tariff
{
    public class TariffBenefitOrdered : BaseEntity
    {
        public int OrderPosition { get; set; }

        public int TariffBenefitId { get; set; }
        public TariffBenefit TariffBenefit { get; set; }
        
        
        public int AdvantageTariffId { get; set; }
        public Tariff AdvantageTariff { get; set; }
        
        public int DisadvantageTariffId { get; set; }
        public Tariff DisadvantageTariff { get; set; }
    }
}