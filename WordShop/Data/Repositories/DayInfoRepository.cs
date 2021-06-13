using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordShop.Data.Interfaces;
using WordShop.Models.DayInfo;

namespace WordShop.Data.Repositories
{
    public class DayInfoRepository : IDayInfoRepository
    {
        private readonly ApplicationDbContext _context;

        public DayInfoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<DayInfo>> GetListOfDayInfosAsync()
        {
            return await _context.DayInfo
                .Include(d => d.DayInfoBlocks.OrderBy(b => b.Id))
                    .ThenInclude(s => s.DayInfoSequenceItems.OrderByDescending(t => t.Id))
                .OrderBy(d => d.Position)
                .ToArrayAsync();
        }
    }
}