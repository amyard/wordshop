using System.Collections.Generic;
using System.Threading.Tasks;
using WordShop.Data.Interfaces;
using WordShop.Models.DayInfo;

namespace WordShop.Data.Repositories
{
    public class DayInfoSequenceItemRepository : IDayInfoSequenceItemRepository
    {
        private readonly ApplicationDbContext _context;

        public DayInfoSequenceItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateSequencesFromArrayAsync(ICollection<string> seqArr, int blockId)
        {
            foreach (var setItem in seqArr)
            {
                DayInfoSequenceItem item = new() { Text = setItem, DayInfoBlockId = blockId };

                await _context.DayInfoSequenceItems.AddAsync(item);
            }

            await _context.SaveChangesAsync();
        }
    }
}