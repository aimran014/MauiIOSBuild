using System.Text.Json.Serialization;

namespace TamsMobile.Models;

public class ProfileResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("messages")]
    public string Messages { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public List<Kakitangan> Data { get; set; } = new();

    [JsonPropertyName("total")]
    public int Total { get; set; }
}

public class Kakitangan
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("userFullname")]
    public string UserFullname { get; set; } = string.Empty;

    [JsonPropertyName("identificationNumber")]
    public string IdentificationNumber { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("accessLevel")]
    public int AccessLevel { get; set; }

    [JsonPropertyName("accessLevelName")]
    public string AccessLevelName { get; set; } = string.Empty;

    [JsonPropertyName("wbType")]
    public string WbType { get; set; } = string.Empty;

    [JsonPropertyName("isLogSub")]
    public bool IsLogSub { get; set; }

    [JsonPropertyName("isLogPelulus")]
    public bool IsLogPelulus { get; set; }

    [JsonPropertyName("isLogPengurusanTertinggi")]
    public bool IsLogPengurusanTertinggi { get; set; }

    [JsonPropertyName("isLogUi")]
    public bool IsLogUi { get; set; }

    [JsonPropertyName("jabatanId")]
    public int JabatanId { get; set; }

    [JsonPropertyName("jabatanCode")]
    public string JabatanCode { get; set; } = string.Empty;

    [JsonPropertyName("jabatanName")]
    public string JabatanName { get; set; } = string.Empty;

    [JsonPropertyName("jabatanState")]
    public string JabatanState { get; set; } = string.Empty;

    [JsonPropertyName("jabatanZone")]
    public string JabatanZone { get; set; } = string.Empty;

    [JsonPropertyName("jabatanReportTo")]
    public string? JabatanReportTo { get; set; }

    [JsonPropertyName("imageType")]
    public string ImageType { get; set; } = string.Empty;

    [JsonPropertyName("imageByte")]
    public byte[]? ImageByte { get; set; }

    [JsonPropertyName("imageBase64")]
    public string ImageBase64 { get; set; } = string.Empty;

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("pelulusId")]
    public string? PelulusId { get; set; }

    [JsonPropertyName("pelulusName")]
    public string? PelulusName { get; set; }

    [JsonPropertyName("pelulusMykad")]
    public string? PelulusMykad { get; set; }

    [JsonPropertyName("isTodayWbBDR")]
    public bool? TodayWbBdrHarian { get; set; }

    [JsonPropertyName("wbBdrId")]
    public string? WbBdrId { get; set; }

    [JsonPropertyName("wbBdrDate")]
    public string? WbBdrDate { get; set; }

    [JsonPropertyName("wbBdrRemarks")]
    public string? WbBdrRemarks { get; set; }

    [JsonPropertyName("periodName")]
    public string? PeriodName { get; set; }
}