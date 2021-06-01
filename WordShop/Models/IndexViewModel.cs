using System.Collections.Generic;

namespace WordShop.Models
{
    public class IndexViewModel
    {
        public string CourseStart { get; set; }
        public IEnumerable<Tariff> Tariffs { get; set; }
    }
}