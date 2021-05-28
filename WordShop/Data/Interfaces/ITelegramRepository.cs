using System.Threading.Tasks;
using WordShop.Enums;
using WordShop.Models;

namespace WordShop.Data.Interfaces
{
    public interface ITelegramRepository
    {
        Task SendNewCustomerMessageToGroup(CustomerInfo customerInfo);
    }
}