using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordShop.Data.Interfaces
{
    public interface IDayInfoSequenceItemRepository
    {
        Task CreateSequencesFromArrayAsync(ICollection<string> seqArr, int blockId);
    }
}