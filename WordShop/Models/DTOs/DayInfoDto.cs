using System.Collections.Generic;

namespace WordShop.Models.DTOs
{
    public class DayInfoDto
    {
        public int DayInfoId { get; set; }
        public string DayName { get; set; }
        public int DayPosition { get; set; }
        public ICollection<DayInfoBlockDto> BlockInfo { get; set; }
    }

    public class DayInfoBlockDto
    {
        public string BlockTitle { get; set; }
        public ICollection<string> Text { get; set; }
    }
}