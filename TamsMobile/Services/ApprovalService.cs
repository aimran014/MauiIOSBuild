using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using TamsMobile.Models;

namespace TamsMobile.Services;

public class ApprovalService : BaseApiService
{
    private readonly ILogger<ApprovalService>? _logger;

    public ApprovalService(HttpClient httpClient, ILogger<ApprovalService>? logger = null)
        : base(httpClient)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get Alasan Rasmi (Official Reasons)
    /// </summary>
    public async Task<AlasanResponse?> GetAlasanRasmiAsync()
    {
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
            {
                _logger?.LogWarning("No auth token found");
                return null;
            }

            SetAuthorizationToken(token);
            var url = $"{BaseUrl}/alasanrasmi";
            
            _logger?.LogInformation("Fetching Alasan Rasmi from: {Url}", url);
            
            var response = await HttpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogWarning("Failed to fetch Alasan Rasmi: {StatusCode}", response.StatusCode);
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<AlasanResponse>();
            _logger?.LogInformation("Fetched {Count} Alasan Rasmi", result?.Total ?? 0);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error fetching Alasan Rasmi");
            return null;
        }
    }

    /// <summary>
    /// Get Alasan Tidak Rasmi (Unofficial Reasons)
    /// </summary>
    public async Task<AlasanResponse?> GetAlasanTidakRasmiAsync()
    {
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
            {
                _logger?.LogWarning("No auth token found");
                return null;
            }

            SetAuthorizationToken(token);
            var url = $"{BaseUrl}/alasantidakrasmi";
            
            _logger?.LogInformation("Fetching Alasan Tidak Rasmi from: {Url}", url);
            
            var response = await HttpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogWarning("Failed to fetch Alasan Tidak Rasmi: {StatusCode}", response.StatusCode);
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<AlasanResponse>();
            _logger?.LogInformation("Fetched {Count} Alasan Tidak Rasmi", result?.Total ?? 0);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error fetching Alasan Tidak Rasmi");
            return null;
        }
    }

    /// <summary>
    /// Get combined list of both Rasmi and Tidak Rasmi reasons
    /// </summary>
    public async Task<List<JenisAlasan>> GetAllAlasanAsync()
    {
        var allReasons = new List<JenisAlasan>();

        var rasmi = await GetAlasanRasmiAsync();
        if (rasmi?.Data != null)
        {
            allReasons.AddRange(rasmi.Data);
        }

        var tidakRasmi = await GetAlasanTidakRasmiAsync();
        if (tidakRasmi?.Data != null)
        {
            allReasons.AddRange(tidakRasmi.Data);
        }

        return allReasons;
    }

    /// <summary>
    /// Get pending approval requests
    /// TODO: Replace with actual API endpoint when available
    /// </summary>
    public async Task<List<PendingApprovalRequest>> GetPendingApprovalsAsync()
    {
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
            {
                _logger?.LogWarning("No auth token found");
                return new List<PendingApprovalRequest>();
            }

            SetAuthorizationToken(token);
            
            // TODO: Replace with actual API endpoint
            // var url = $"{BaseUrl}/permohonan/pending";
            // var response = await HttpClient.GetAsync(url);
            
            _logger?.LogInformation("Fetching pending approvals...");
            
            // Placeholder - replace with actual API call
            return new List<PendingApprovalRequest>();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error fetching pending approvals");
            return new List<PendingApprovalRequest>();
        }
    }

    /// <summary>
    /// Approve a request
    /// TODO: Replace with actual API endpoint when available
    /// </summary>
    public async Task<(bool Success, string Message)> ApproveRequestAsync(string requestId, string approverNotes)
    {
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
            {
                return (false, "Token tidak dijumpai");
            }

            SetAuthorizationToken(token);
            
            // TODO: Replace with actual API endpoint
            // var url = $"{BaseUrl}/permohonan/approve";
            // var payload = new { RequestId = requestId, Notes = approverNotes };
            // var response = await HttpClient.PostAsJsonAsync(url, payload);
            
            _logger?.LogInformation("Approving request: {RequestId}", requestId);
            
            // Placeholder
            await Task.Delay(500);
            return (true, "Permohonan telah diluluskan");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error approving request");
            return (false, "Gagal meluluskan permohonan");
        }
    }

    /// <summary>
    /// Reject a request
    /// TODO: Replace with actual API endpoint when available
    /// </summary>
    public async Task<(bool Success, string Message)> RejectRequestAsync(string requestId, string rejectionReason)
    {
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
            {
                return (false, "Token tidak dijumpai");
            }

            SetAuthorizationToken(token);
            
            // TODO: Replace with actual API endpoint
            // var url = $"{BaseUrl}/permohonan/reject";
            // var payload = new { RequestId = requestId, Reason = rejectionReason };
            // var response = await HttpClient.PostAsJsonAsync(url, payload);
            
            _logger?.LogInformation("Rejecting request: {RequestId}", requestId);
            
            // Placeholder
            await Task.Delay(500);
            return (true, "Permohonan telah ditolak");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error rejecting request");
            return (false, "Gagal menolak permohonan");
        }
    }
}