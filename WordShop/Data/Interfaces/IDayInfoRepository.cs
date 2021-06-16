using System.Collections.Generic;
using System.Threading.Tasks;
using WordShop.Models.DayInfo;
using WordShop.Models.DTOs;

namespace WordShop.Data.Interfaces
{
    public interface IDayInfoRepository
    {
        Task<IEnumerable<DayInfo>> GetListOfDayInfosAsync();
        Task<int> CreateDayInfoAsync(DayInfoDto dayInfoDto);
    }
}