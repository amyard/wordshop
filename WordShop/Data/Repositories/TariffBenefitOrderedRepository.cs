using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordShop.Data.Interfaces;

namespace WordShop.Data.Repositories
{
    public class TariffBenefitOrderedRepository : ITariffBenefitOrderedRepository
    {
        private readonly ApplicationDbContext _context;

        public TariffBenefitOrderedRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<int[]> GetBenefitsByTariffId(int tariffId)
        {
            return await _context.TariffBenefitOrdered
                .Where(x => x.AdvantageTariffId == tariffId || x.DisadvantageTariffId == tariffId)
                .Select(x => x.TariffBenefitId)
                .ToArrayAsync();
        }
    }
}