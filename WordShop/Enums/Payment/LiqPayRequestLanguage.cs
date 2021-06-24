using System.Runtime.Serialization;

namespace WordShop.Enums.Payment
{
    public enum LiqPayRequestLanguage
    {
        [EnumMember(Value = "ru")]
        RU,
        [EnumMember(Value = "uk")]
        UK,
        [EnumMember(Value = "en")]
        EN
    }
}