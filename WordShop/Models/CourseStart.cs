using System;
using WordShop.Enums;

namespace WordShop.Models
{
    public class CourseStart : BaseEntity
    {
        public Courses Courses { get; set; } = Courses.WordShop;
        public DateTime CourseStartDate { get; set; }
    }
}