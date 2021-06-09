using System.Threading.Tasks;

namespace WordShop.Data.Interfaces
{
    public interface ITariffBenefitOrderedRepository
    {
        Task<int[]> GetBenefitsByTariffId(int tariffId);
    }
}