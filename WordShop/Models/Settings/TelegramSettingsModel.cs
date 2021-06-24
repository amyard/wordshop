namespace WordShop.Models.Settings
{
    public class TelegramSettingsModel
    {
        public string ApiToken { get; set; }
        public long ChatIdGroup { get; set; }
        public long DevChatId { get; set; }
        public long AnnaChatId { get; set; }
    }
}