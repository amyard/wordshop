using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordShop.Data.Interfaces;
using WordShop.Models.DayInfo;
using WordShop.Models.DTOs;

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

        public async Task<int> CreateDayInfoAsync(DayInfoDto dayInfoDto)
        {
            DayInfo dayInfo = new DayInfo
            {
                Title = dayInfoDto.DayName,
                Position =  dayInfoDto.DayPosition
            };

            await _context.DayInfo.AddAsync(dayInfo);
            await _context.SaveChangesAsync();

            return dayInfo.Id;
        }

        public async Task DeleteDayInfoAsync(int dayId)
        {
            var dayInfo = await _context.DayInfo.Where(x => x.Id == dayId).FirstOrDefaultAsync();

            if (dayInfo != null)
            {
                _context.DayInfo.Remove(dayInfo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<DayInfo> GetDayInfoByIdAsync(int id)
        {
            var dayInfo = await _context.DayInfo.Where(x => x.Id == id)
                .Include(x => x.DayInfoBlocks)
                .ThenInclude(y => y.DayInfoSequenceItems)
                .FirstOrDefaultAsync();

            return dayInfo;
        }
    }
}