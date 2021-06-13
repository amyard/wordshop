using System.Threading.Tasks;
using WordShop.Enums;
using WordShop.Models;
using WordShop.Models.CustomerInfo;

namespace WordShop.Data.Interfaces
{
    public interface ITelegramRepository
    {
        Task SendNewCustomerMessageToGroup(CustomerInfo customerInfo);
    }
}