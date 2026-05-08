using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using TamsMobile.Models;

namespace TamsMobile.Services;

public class ProfileService : BaseApiService
{
    private readonly ILogger<ProfileService>? _logger;

    public ProfileService(HttpClient httpClient, ILogger<ProfileService>? logger = null) 
        : base(httpClient)
    {
        _logger = logger;
    }

    public async Task<Kakitangan?> GetUserProfileAsync(string identificationNumber)
    {
        try
        {
            _logger?.LogInformation("Loading user profile for IC: {IC}", identificationNumber);

            var profileUrl = $"{BaseUrl}/profile";
            _logger?.LogInformation("Sending GET request to: {ProfileUrl}", profileUrl);

            var response = await HttpClient.GetAsync(profileUrl);
            _logger?.LogInformation("Profile Response Status: {StatusCode}", response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger?.LogDebug("Profile Content Length: {Length}", responseContent.Length);

                var result = await response.Content.ReadFromJsonAsync<ProfileResponse>();

                if (result?.Success == true && result.Data?.Count > 0)
                {
                    var userProfile = result.Data.FirstOrDefault(k =>
                        k.IdentificationNumber == identificationNumber);

                    if (userProfile != null)
                    {
                        _logger?.LogInformation("Profile loaded: {Fullname}", userProfile.UserFullname);
                        return userProfile;
                    }

                    _logger?.LogWarning("User profile not found for IC: {IC}", identificationNumber);
                }
                else
                {
                    _logger?.LogWarning("Profile load failed. Success: {Success}, Count: {Count}",
                        result?.Success, result?.Data?.Count);
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger?.LogError("Profile Error: {ErrorContent}", errorContent);
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error loading profile");
            return null;
        }
    }

    public Task<string> GetProfileImageSourceAsync(Kakitangan kakitangan)
    {
        var profile = kakitangan;
        if (profile == null)
            return Task.FromResult(string.Empty);

        var base64 = profile.ImageBase64?.Trim();
        if (string.IsNullOrWhiteSpace(base64))
            return Task.FromResult(string.Empty);

        // already formatted image
        if (base64.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            return Task.FromResult(base64);

        var imageType = NormalizeImageType(profile.ImageType);

        var result = $"data:{imageType};base64,{base64}";

        return Task.FromResult(result);
    }

    private string NormalizeImageType(string? imageType)
    {
        if (string.IsNullOrWhiteSpace(imageType)) return "image/jpeg";

        return imageType.ToLowerInvariant() switch
        {
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ when imageType.StartsWith("image/", StringComparison.OrdinalIgnoreCase) => imageType,
            _ => "image/jpeg"
        };
    }
}