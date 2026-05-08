using System.Text.Json.Serialization;

namespace TamsMobile.Models;

// Alasan Rasmi & Tidak Rasmi Response
public class AlasanResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("messages")]
    public string Messages { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public List<JenisAlasan>? Data { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}

public class JenisAlasan
{
    [JsonPropertyName("jenis_id")]
    public string? JenisId { get; set; }

    [JsonPropertyName("jenis_name")]
    public string? JenisName { get; set; }

    [JsonPropertyName("jenis_status")]
    public string? JenisStatus { get; set; }

    [JsonPropertyName("alasan_id")]
    public string? AlasanId { get; set; }

    [JsonPropertyName("alasan_name")]
    public string? AlasanName { get; set; }

    [JsonPropertyName("status_id")]
    public string? StatusId { get; set; }

    // Display helper
    public string DisplayName => $"{AlasanName} ({JenisName})";
}

// Pending Approval Request Model
public class PendingApprovalRequest
{
    public string RequestId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string FormattedDate => Date.ToString("dd MMM yyyy");
    public string TimeIn { get; set; } = string.Empty;
    public string TimeOut { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty; // "Rasmi" or "Tidak Rasmi"
    public string Status { get; set; } = "Pending";
}