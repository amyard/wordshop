using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordShop.Data.Interfaces;
using WordShop.Enums;
using WordShop.Models;

namespace WordShop.Data.Repositories
{
    public class TariffRepository : ITariffRepository
    {
        private readonly ApplicationDbContext _context;

        public TariffRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Tariff>> GetAllTariffsAsync(Courses courses, Level level)
        {
            return await _context.Tariffs
                .Include(t => t.Advantage.OrderBy(a => a.OrderPosition))
                    .ThenInclude(t => t.TariffBenefit)
                .Include(t => t.Disadvantage.OrderBy(d => d.OrderPosition))
                    .ThenInclude(t => t.TariffBenefit)
                .Where(t => t.Courses == courses && t.Level == level)
                .ToListAsync();
        }

        public async Task<Tariff> GetTariffByIdAsync(int id)
        {
            return await _context.Tariffs
                .Include(t => t.Advantage.OrderBy(a => a.OrderPosition))
                    .ThenInclude(t => t.TariffBenefit)
                .Include(t => t.Disadvantage.OrderBy(d => d.OrderPosition))
                    .ThenInclude(t => t.TariffBenefit)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task SaveTariffAsync(Tariff tariff)
        {
            await _context.Tariffs.AddAsync(tariff);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsTariffExists(int tariffId, Courses courses, Level level)
        {
            return await _context.Tariffs.AnyAsync(t => t.Id == tariffId && t.Courses == courses && t.Level == level);
        }

        public async Task UpdateTariffAsync(Tariff tariff)
        {
            var tariffFromDB = await _context.Tariffs.Where(x => x.Id == tariff.Id).FirstOrDefaultAsync();

            tariffFromDB.Name = tariff.Name;
            tariffFromDB.OldPrice = tariff.OldPrice;
            tariffFromDB.NewPrice = tariff.NewPrice;
        }
    }
}