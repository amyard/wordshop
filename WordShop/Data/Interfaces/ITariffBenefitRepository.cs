using System.Collections.Generic;
using System.Threading.Tasks;
using WordShop.Models;
using WordShop.Models.Tariff;

namespace WordShop.Data.Interfaces
{
    public interface ITariffBenefitRepository
    {
        Task<IEnumerable<TariffBenefit>> GetTariffBenefitsList();
        Task<IEnumerable<TariffBenefit>> GetTariffBenefitsListWithoutInUseIds(int[] ids);
        Task<TariffBenefit> GetTariffBenefit(int id);
        Task CreateTariffBenefit(TariffBenefit benefit);
        Task UpdateTariffBenefit(TariffBenefit benefit);
        Task<bool> SaveAllAsync();
    }
}