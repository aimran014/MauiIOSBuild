using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TamsMobile.Models;
using static TamsMobile.Models.ReportingModel;

namespace TamsMobile.Services
{
    public class ReportingService : BaseApiService
    {
        private readonly ILogger<ReportingService>? _logger;

        public ReportingService(HttpClient httpClient, ILogger<ReportingService>? logger = null)
            : base(httpClient)
        {
            _logger = logger;
        }

        public Task<MobileResponse<List<ReportBulananIndividuResponse>>> GetReportBulananIndividuAsync(ReportBulananIndividuRequest request)
        {
            return GetReportBulananIndividu(request);
        }

        private async Task<MobileResponse<List<ReportBulananIndividuResponse>>> GetReportBulananIndividu(ReportBulananIndividuRequest request)
        {
            // Implementation for fetching the report
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

                var ReportIndividuUrl = $"{BaseUrl}/reportbulananindividu";
                var response = await HttpClient.PostAsJsonAsync(ReportIndividuUrl, request);

                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    // ❌ Failed response
                    return new MobileResponse<List<ReportBulananIndividuResponse>>
                    {
                        Success = false,
                        Messages = $"Request failed: {response.StatusCode} - {responseBody}"
                    };

                }

                var reportData = await response.Content.ReadFromJsonAsync<MobileResponse<List<ReportBulananIndividuResponse>>>();
                if (reportData == null)
                {
                    _logger?.LogWarning("Response deserialization returned null. Response body: {ResponseBody}", responseBody);
                    return new MobileResponse<List<ReportBulananIndividuResponse>>
                    {
                        Success = false,
                        Messages = "Failed to parse report data from response."
                    };
                }
                else
                {
                    _logger?.LogInformation("Report data successfully retrieved and parsed. Total records: {TotalRecords}", reportData.Data?.Count ?? 0);
                    return reportData;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error fetching report bulanan individu.");
                return new MobileResponse<List<ReportBulananIndividuResponse>>
                {
                    Success = false,
                    Messages = "An error occurred while fetching the report.",
                    Data = null
                };
            }

        }
    }
}
