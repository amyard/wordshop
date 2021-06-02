using System.Collections.Generic;
using System.Threading.Tasks;
using WordShop.Enums;
using WordShop.Models;

namespace WordShop.Data.Interfaces
{
    public interface ITariffRepository
    {
        Task<IEnumerable<Tariff>> GetAllTariffsAsync(Courses courses, Level level);
        Task<Tariff> GetTariffByIdAsync(int id);
        Task SaveTariffAsync(Tariff tariff);
        Task<bool> SaveAllAsync();
        Task<bool> IsTariffExists(int tariffId, Courses courses, Level level);
    }
}