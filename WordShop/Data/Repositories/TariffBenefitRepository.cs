using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordShop.Data.Interfaces;
using WordShop.Enums;
using WordShop.Models;

namespace WordShop.Data.Repositories
{
    public class TariffBenefitRepository : ITariffBenefitRepository
    {
        private readonly ApplicationDbContext _context;

        public TariffBenefitRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<TariffBenefit>> GetTariffBenefitsList()
        {
            return await _context.TariffBenefits.ToListAsync();
        }

        public async Task<TariffBenefit> GetTariffBenefit(int id)
        {
            return await _context.TariffBenefits.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateTariffBenefit(TariffBenefit benefit)
        {
            await _context.TariffBenefits.AddAsync(benefit);
        }

        public async Task UpdateTariffBenefit(TariffBenefit benefit)
        {
            TariffBenefit tariff = await this.GetTariffBenefit(benefit.Id);

            if (tariff != null) tariff.Benefit = benefit.Benefit;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}