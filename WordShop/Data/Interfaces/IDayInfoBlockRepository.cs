using System.Threading.Tasks;

namespace WordShop.Data.Interfaces
{
    public interface IDayInfoBlockRepository
    {
        Task<int> CreateDayInfoBlockAsync(string title, int dayInfoId);
    }
}