using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordShop.Data.Interfaces;
using WordShop.Enums;
using WordShop.Models;

namespace WordShop.Data.Repositories
{
    public class CustomerInfoRepository : ICustomerInfoRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerInfoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<CustomerInfo>> GetAllCustomersAsync()
        {
            return await _context.CustomerInfos.ToListAsync();
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