using System.Collections.Generic;
using System.Threading.Tasks;
using WordShop.Models.DayInfo;

namespace WordShop.Data.Interfaces
{
    public interface IDayInfoRepository
    {
        Task<IEnumerable<DayInfo>> GetListOfDayInfosAsync();
    }
}