using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using TamsMobile.Models;

namespace TamsMobile.Services;

public class AuthenticationService : BaseApiService
{
    private readonly ILogger<AuthenticationService>? _logger;

    public AuthenticationService(HttpClient httpClient, ILogger<AuthenticationService>? logger = null)
        : base(httpClient)
    {
        _logger = logger;
    }

    public async Task<(bool Success, string Message, UserData? UserData, string? Token)> LoginAsync(
        string mykad, string password)
    {
        try
        {
            _logger?.LogInformation("=== LOGIN ATTEMPT ===");
            _logger?.LogInformation("Base URL: {BaseUrl}", HttpClient.BaseAddress);
            _logger?.LogInformation("MyKad: {Mykad}", mykad);

            var request = new LoginRequest
            {
                Mykad = mykad,
                Password = password
            };

            var loginUrl = $"{BaseUrl}/login";
            _logger?.LogInformation("Sending POST request to: {LoginUrl}", loginUrl);

            var response = await HttpClient.PostAsJsonAsync(loginUrl, request);
            _logger?.LogInformation("Response Status: {StatusCode}", response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger?.LogDebug("Response Content: {Content}", responseContent);

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (result?.IsSuccess == true && result.Data != null)
                {
                    _logger?.LogInformation("Login successful! Token received");

                    //SAVE TOKEN TO SECURE STORAGE
                    await SecureStorage.SetAsync("auth_token", result.Token);
                    _logger?.LogInformation("Token saved to SecureStorage");

                    // Set authorization header for this service's requests
                    SetAuthorizationToken(result.Token);

                    return (true, result.Message, result.Data, result.Token);
                }

                return (false, result?.Message ?? "Login gagal", null, null);
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger?.LogError("Error Content: {ErrorContent}", errorContent);

            return (false, $"Ralat pelayan: {response.StatusCode}", null, null);
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "HTTP Request Exception during login");
            return (false, $"Ralat rangkaian: {ex.Message}", null, null);
        }
        catch (TaskCanceledException ex)
        {
            _logger?.LogError(ex, "Request timeout during login");
            return (false, "Sambungan tamat masa. Sila cuba lagi.", null, null);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Unexpected error during login");
            return (false, $"Ralat: {ex.Message}", null, null);
        }
    }

    public async Task LogoutAsync()
    {
        _logger?.LogInformation("User logging out...");

        //REMOVE TOKEN FROM STORAGE
        SecureStorage.Remove("auth_token");
        _logger?.LogInformation("Token removed from SecureStorage");

        ClearAuthorizationToken();
    }
}