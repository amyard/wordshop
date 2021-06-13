using System.Collections.Generic;

namespace WordShop.Models.ViewModels
{
    public class IndexViewModel
    {
        public string CourseStart { get; set; }
        public IEnumerable<Tariff.Tariff> Tariffs { get; set; }
    }
}