using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordShop.Data.Interfaces;
using WordShop.Models;

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

        public async Task DeleteBenefitByTariffId(int tariffId)
        {
            var deletedData = await _context.TariffBenefitOrdered
                .Where(x => x.AdvantageTariffId == tariffId || x.DisadvantageTariffId == tariffId)
                .ToArrayAsync();
                
            _context.TariffBenefitOrdered.RemoveRange(deletedData);

            await _context.SaveChangesAsync();
        }

        public async Task SaveTariffBenefitRange(List<TariffBenefitOrdered> data)
        {
            // foreach (var item in data)
            // {
            //     var t1 = new TariffBenefitOrdered
            //     {
            //         OrderPosition = item.OrderPosition,
            //         TariffBenefitId = item.TariffBenefitId,
            //         AdvantageTariffId = item.AdvantageTariffId,
            //         DisadvantageTariffId = item.DisadvantageTariffId
            //     };
            //
            //     await _context.TariffBenefitOrdered.AddAsync(t1);
            // }

            await _context.TariffBenefitOrdered.AddRangeAsync(data);
        }
        
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}