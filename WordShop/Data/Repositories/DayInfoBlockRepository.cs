using System.Threading.Tasks;
using WordShop.Data.Interfaces;
using WordShop.Models.DayInfo;

namespace WordShop.Data.Repositories
{
    public class DayInfoBlockRepository : IDayInfoBlockRepository
    {
        private readonly ApplicationDbContext _context;

        public DayInfoBlockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateDayInfoBlockAsync(string title, int dayInfoId)
        {
            DayInfoBlock block = new DayInfoBlock
            {
                Title = title, DayInfoId = dayInfoId
            };

            await _context.DayInfoBlocks.AddAsync(block);
            await _context.SaveChangesAsync();

            return block.Id;
        }
    }
}