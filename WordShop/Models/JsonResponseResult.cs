using WordShop.Enums;

namespace WordShop.Models
{
    public class JsonResponseResult
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public ErrorTypes Error { get; set; } = ErrorTypes.None;
    }
}