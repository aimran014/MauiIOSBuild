using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using TamsMobile.Models;

namespace TamsMobile.Services;

public class AttendanceService : BaseApiService
{
    private readonly ILogger<AttendanceService>? _logger;

    public AttendanceService(HttpClient httpClient, ILogger<AttendanceService>? logger = null)
        : base(httpClient)
    {
        _logger = logger;
    }

    public Task<List<AttendanceRecord>> GetAttendanceRecordsAsync()
    {
        return GetAttendanceRecordsInternalAsync(null);
    }

    public Task<List<AttendanceRecord>> GetAttendanceRecordsAsync(DateTime selectedDate)
    {
        return GetAttendanceRecordsInternalAsync(selectedDate);
    }

    public Task<MobileResponse<AttendanceSummary>> GetAttendanceSummaryAsync(int month, int year)
    {
        return GetAttendanceSummary(month, year);
    }

    public Task<MobileResponse<AttendanceRecord>> GetAttendanceDetailsAsync(int attendanceId)
    {
        return GetAttendanceDetails(attendanceId);
    }

    public Task<MobileResponse<List<RekodImbasanModel>>> GetRekodImbasanKakitanganAsync()
    {
        return GetRekodImbasanKakitangan();
    }

    public Task<MobileResponse<string>> CaptureAttendanceAsync(string mykad, string latitude, string longitude, string fullAddress)
    {
        return CaptureAttendance(mykad, latitude, longitude, fullAddress);
    }

    private async Task<List<AttendanceRecord>> GetAttendanceRecordsInternalAsync(DateTime? selectedDate)
    {
        try
        {
            //  RETRIEVE TOKEN FROM SECURE STORAGE
            var token = await SecureStorage.GetAsync("auth_token");

            if (string.IsNullOrEmpty(token))
            {
                _logger?.LogWarning("No auth token found in SecureStorage. User needs to log in.");
                return new List<AttendanceRecord>();
            }

            //  SET TOKEN ON THIS HTTP CLIENT
            SetAuthorizationToken(token);
            _logger?.LogInformation("Authorization token set from storage");

            var attendanceUrl = $"{BaseUrl}/kehadiran";

            if (selectedDate.HasValue)
            {
                var dateParam = selectedDate.Value.ToString("yyyy-MM");
                attendanceUrl += $"?SelectedDate={dateParam}";
            }

            _logger?.LogInformation("Requesting attendance for: {DateParam}",
                selectedDate?.ToString("yyyy-MM") ?? "current month");
            _logger?.LogInformation("Full URL: {Url}", attendanceUrl);

            var response = await HttpClient.GetAsync(attendanceUrl);
            var content = await response.Content.ReadAsStringAsync();

            _logger?.LogInformation("Status: {Status}, Response length: {Length}",
                response.StatusCode, content.Length);
            _logger?.LogInformation("Raw response: {Content}", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogWarning("Non-success status code: {StatusCode}", response.StatusCode);
                return new List<AttendanceRecord>();
            }

            var result = await response.Content.ReadFromJsonAsync<AttendanceResponse>();

            _logger?.LogInformation("API Response - Success: {Success}, Message: {Message}, Count: {Count}",
                result?.Success, result?.Messages, result?.RekodKehadiran?.Count ?? 0);

            if (result?.RekodKehadiran != null && result.RekodKehadiran.Count > 0)
            {
                _logger?.LogInformation("First record: {Date}", result.RekodKehadiran[0].MyDate);
            }

            return result?.RekodKehadiran?  
                .OrderBy(r => r.ParsedDate)
                .ToList()
                ?? new List<AttendanceRecord>();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Attendance load failed for date: {Date}",
                selectedDate?.ToString("yyyy-MM") ?? "null");
            return new List<AttendanceRecord>();
        }
    }

    private async Task<MobileResponse<AttendanceSummary>> GetAttendanceSummary(int months, int years)
    {
        try
        {
            //  RETRIEVE TOKEN FROM SECURE STORAGE
            var token = await SecureStorage.GetAsync("auth_token");

            if (string.IsNullOrEmpty(token))
            {
                _logger?.LogWarning("No auth token found in SecureStorage. User needs to log in.");
                return new();
            }

            //  SET TOKEN ON THIS HTTP CLIENT
            SetAuthorizationToken(token);
            _logger?.LogInformation("Authorization token set from storage");

            var attendanceUrl = $"{BaseUrl}/summarykehadiran";
            attendanceUrl += $"?Bulan={months}&Tahun={years}";

            var response = await HttpClient.GetAsync(attendanceUrl);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogWarning("Non-success status code: {StatusCode}", response.StatusCode);
                return new();
            }

            var result = await response.Content.ReadFromJsonAsync <MobileResponse<AttendanceSummary>>();

            _logger?.LogInformation("API Response - Success: {Success}, Message: {Message}",
            result?.Success,
            result?.Messages);

            return result ?? new MobileResponse<AttendanceSummary>();

        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to load attendance summary for {Month}/{Year}", months, years);
            return new();
        }
    }

    private async Task<MobileResponse<AttendanceRecord>> GetAttendanceDetails(int AttendanceId)
    {
        try
        {
            //  RETRIEVE TOKEN FROM SECURE STORAGE
            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
            {
                _logger?.LogWarning("No auth token found in SecureStorage. User needs to log in.");
                return new();
            }
            //  SET TOKEN ON THIS HTTP CLIENT
            SetAuthorizationToken(token);
            _logger?.LogInformation("Authorization token set from storage");
            var attendanceUrl = $"{BaseUrl}/kehadirandetails?AttendanceId={AttendanceId}";
            var response = await HttpClient.GetAsync(attendanceUrl);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogWarning("Non-success status code: {StatusCode}", response.StatusCode);
                return new();
            }
            var result = await response.Content.ReadFromJsonAsync<MobileResponse<AttendanceRecord>>();
            _logger?.LogInformation("API Response - Success: {Success}, Message: {Message}",
                result?.Success, result?.Messages);
            return result ?? new MobileResponse<AttendanceRecord>();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to load attendance details for ID: {Id}", AttendanceId);
            return new();
        }
    }

    private async Task<MobileResponse<List<RekodImbasanModel>>>GetRekodImbasanKakitangan()
    {
        try
        {
            //  RETRIEVE TOKEN FROM SECURE STORAGE
            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
            {
                _logger?.LogWarning("No auth token found in SecureStorage. User needs to log in.");
                return new();
            }
            //  SET TOKEN ON THIS HTTP CLIENT
            SetAuthorizationToken(token);
            _logger?.LogInformation("Authorization token set from storage");

            var attendanceUrl = $"{BaseUrl}/rekodimbasan";
            var response = await HttpClient.GetAsync(attendanceUrl);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogWarning("Non-success status code: {StatusCode}", response.StatusCode);
                return new();
            }
            var result = await response.Content.ReadFromJsonAsync<MobileResponse<List<RekodImbasanModel>>>();
            _logger?.LogInformation("API Response - Success: {Success}, Message: {Message}",
                result?.Success, result?.Messages);
            return result ?? new MobileResponse<List<RekodImbasanModel>>();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to load imbasan records");
            return new();
        }
    }

    private async Task<MobileResponse<string>> CaptureAttendance(string mykad, string latitude, string longitude, string fullAddress)
    {
        try
        {
            //  RETRIEVE TOKEN FROM SECURE STORAGE
            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
            {
                _logger?.LogWarning("No auth token found in SecureStorage. User needs to log in.");
                return new();
            }
            //  SET TOKEN ON THIS HTTP CLIENT
            SetAuthorizationToken(token);
            _logger?.LogInformation("Authorization token set from storage");

            var attendanceUrl = $"{BaseUrl}/capturekehadiran";

            var requestData = new
            {
                siteName = "KETSA",
                mykad = mykad.Trim(),
                latLng = $"{latitude},{longitude}",
                fullAddress = fullAddress.Trim()
            };

            // ✅ Send request
            var response = await HttpClient.PostAsJsonAsync(attendanceUrl, requestData);

            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // ❌ Failed response
                return new MobileResponse<string>
                {
                    Success = false,
                    Messages = $"Request failed: {response.StatusCode} - {responseBody}"
                };

            }

            return new MobileResponse<string>
            {
                Success = true,
                Messages = responseBody // or "Success"
            };
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to capture attendance for MyKad: {MyKad}", mykad);
            return new();
        }
    }
}