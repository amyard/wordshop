using System.Collections.Generic;
using System.Threading.Tasks;
using WordShop.Models;

namespace WordShop.Data.Interfaces
{
    public interface ITariffBenefitRepository
    {
        Task<IEnumerable<TariffBenefit>> GetTariffBenefitList();
        Task<Tariff> GetTariffBenefit();
        void CreateTariffBenefit();
        void UpdateTariffBenefit();
        Task<bool> SaveAllAsync();
    }
}