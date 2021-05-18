using System.Collections.Generic;
using System.Threading.Tasks;
using WordShop.Models;

namespace WordShop.Data.Interfaces
{
    public interface ICustomerInfoRepository
    {
        Task<IEnumerable<CustomerInfo>> GetAllCustomersAsync();
        Task SaveCustomerInfoAsync(CustomerInfo customer);
        Task<bool> SaveAllAsync();
        Task<bool> IsEmailUnique(string email);
    }
}