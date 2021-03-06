using System.Collections.Generic;
using System.Threading.Tasks;
using WordShop.Models;
using WordShop.Models.Tariff;

namespace WordShop.Data.Interfaces
{
    public interface ITariffBenefitOrderedRepository
    {
        Task<int[]> GetBenefitsByTariffId(int tariffId);
        Task DeleteBenefitByTariffId(int tariffId);
        Task SaveTariffBenefitRange(List<TariffBenefitOrdered> data);
        Task<bool> SaveAllAsync();
    }
}