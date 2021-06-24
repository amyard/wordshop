using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordShop.Data.Interfaces;
using WordShop.Enums;
using WordShop.Models.CustomerInfo;
using WordShop.Models.DTOs;

namespace WordShop.Data.Repositories
{
    public class CustomerInfoRepository : ICustomerInfoRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerInfoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<CustomerInfoDto>> GetAllCustomersAsync()
        {
            return await _context.CustomerInfos
                .Include(t => t.Tariff)
                .Select(x => new CustomerInfoDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    TariffName = x.Tariff.Name,
                    TariffNewPrice = "$"+x.Tariff.NewPrice.ToString(),
                    Courses = x.Courses.ToString(),
                    CreatedDate = x.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss"),
                    PaymentStatus = x.PaymentStatus.ToString(),
                    OrderId = x.OrderId.ToString()
                })
                .OrderByDescending(x => x.Id)
                .ToListAsync();
        }

        public async Task SaveCustomerInfoAsync(CustomerInfo customer)
        {
            await _context.CustomerInfos.AddAsync(customer);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsEmailUnique(string email)
        {
            return await _context.CustomerInfos.AnyAsync(x => x.Email == email.ToLower());
        }

        public async Task<bool> IsEmailUniqueByCourseAndTariff(string email, Courses courses, Level level, int tariffId)
        {
            var result = await _context.CustomerInfos.AnyAsync(x => x.Email == email.ToLower() &&
                                                                    x.Courses == courses && 
                                                                    x.CourseLevel == level);
            return result;
        }
    }
}