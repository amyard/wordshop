using System.Collections.Generic;

namespace WordShop.Models.DayInfo
{
    public class DayInfo : BaseEntity
    {
        public string Title { get; set; }
        public int Position { get; set; }

        public IList<DayInfoBlock> DayInfoBlocks { get; set; }
    }
}