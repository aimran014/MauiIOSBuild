using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TamsMobile.Models
{
    public class UrusanModel
    {
        [JsonPropertyName("jenis_id")]
        public string? UrusanId { get; set; }

        [JsonPropertyName("jenis_name")]
        public string? UrusanName { get; set; }

        [JsonPropertyName("jenis_status")]
        public string? UrusanStatus { get; set; }

        [JsonPropertyName("alasan_id")]
        public string? AlasanId { get; set; }

        [JsonPropertyName("alasan_name")]
        public string? AlasanName { get; set; }

        [JsonPropertyName("status_id")]
        public string? AlasanStatus { get; set; }

    }
}
