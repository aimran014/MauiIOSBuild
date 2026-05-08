using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TamsMobile.Models
{
    public class MobileResponses<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("messages")]
        public string Messages { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public List<T>? Data { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

    }
    public class MobileResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("messages")]
        public string Messages { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public T? Data { get; set; }

    }
}
