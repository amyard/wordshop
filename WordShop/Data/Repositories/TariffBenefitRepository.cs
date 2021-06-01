using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordShop.Data.Interfaces;
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
        
        public async Task<IEnumerable<TariffBenefit>> GetTariffBenefitList()
        {
            return await _context.TariffBenefits.ToListAsync();
        }

        public Task<Tariff> GetTariffBenefit()
        {
            throw new System.NotImplementedException();
        }

        public void CreateTariffBenefit()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateTariffBenefit()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}