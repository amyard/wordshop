using Newtonsoft.Json;

namespace WordShop.Models.Payment
{
    public class PaymentStatusResponse
    {
        [JsonProperty("data")]
        public string Data { get; set; }
        
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}