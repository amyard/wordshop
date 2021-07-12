using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordShop.Enums;
using WordShop.Models.CustomerInfo;
using WordShop.Models.DTOs;

namespace WordShop.Data.Interfaces
{
    public interface ICustomerInfoRepository
    {
        Task<IEnumerable<CustomerInfoDto>> GetAllCustomersAsync();
        Task<CustomerInfo> GetCustomerByGuidAsync(Guid orderId);
        Task SaveCustomerInfoAsync(CustomerInfo customer);
        Task<bool> SaveAllAsync();
        Task<bool> IsEmailUnique(string email);
        Task<bool> IsEmailUniqueByCourseAndTariff(string email, Courses courses, Level level, int tariffId);
    }
}