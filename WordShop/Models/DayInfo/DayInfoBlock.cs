using System.Collections.Generic;

namespace WordShop.Models.DayInfo
{
    public class DayInfoBlock : BaseEntity
    {
        public string Title { get; set; }

        public int DayInfoId { get; set; }
        public DayInfo DayInfo { get; set; }
        
        public ICollection<DayInfoSequenceItem> DayInfoSequenceItems { get; set; }
    }
}