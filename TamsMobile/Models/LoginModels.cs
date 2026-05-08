using System.Text.Json.Serialization;

namespace TamsMobile.Models;

public class LoginRequest
{
    [JsonPropertyName("mykad")]
    public string Mykad { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; set; }

    [JsonPropertyName("data")]
    public UserData? Data { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;
}

public class UserData
{
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("identificationNumber")]
    public string IdentificationNumber { get; set; } = string.Empty;

    [JsonPropertyName("token")]
    public string? Token { get; set; }

    [JsonPropertyName("userId")]
    public string UserId { get; set; } = string.Empty;

    [JsonPropertyName("accessLevel")]
    public string AccessLevel { get; set; } = string.Empty;

    [JsonPropertyName("isPelulus")]
    public bool IsPelulus { get; set; }

    [JsonPropertyName("isLogPengurusanTertinggi")]
    public bool IsLogPengurusanTertinggi { get; set; }

    [JsonPropertyName("isLogSub")]
    public bool IsLogSub { get; set; }

    [JsonPropertyName("isLogUi")]
    public bool IsLogUi { get; set; }

    [JsonPropertyName("wbType")]
    public string WbType { get; set; } = string.Empty;
}