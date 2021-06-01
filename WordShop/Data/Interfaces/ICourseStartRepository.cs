using System.Threading.Tasks;
using WordShop.Models;

namespace WordShop.Data.Interfaces
{
    public interface ICourseStartRepository
    {
        CourseStart GetCourseStart();
        void UpdateCourseStartAsync(CourseStart courseStart);
        Task<bool> SaveAllAsync();
    }
}