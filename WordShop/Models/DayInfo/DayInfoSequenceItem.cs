namespace WordShop.Models.DayInfo
{
    public class DayInfoSequenceItem : BaseEntity
    {
        public string Text { get; set; }
        
        public int DayInfoBlockId { get; set; }
        public virtual DayInfoBlock DayInfoBlock { get; set; }
    }
}