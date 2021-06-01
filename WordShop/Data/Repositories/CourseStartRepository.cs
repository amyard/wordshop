using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordShop.Data.Interfaces;
using WordShop.Models;

namespace WordShop.Data.Repositories
{
    public class CourseStartRepository : ICourseStartRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseStartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public CourseStart GetCourseStart()
        {
            return _context.CourseStarts.OrderByDescending(x => x.Id).FirstOrDefault();
        }

        public void UpdateCourseStartAsync(CourseStart courseStart)
        {
            var entity = this.GetCourseStart();
            if (entity != null) entity.CourseStartDate = courseStart.CourseStartDate;

            _context.SaveChanges();
        }
        
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}