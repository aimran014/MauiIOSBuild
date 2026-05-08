using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TamsMobile.Models;

namespace TamsMobile.Services
{
    public  class UrusanService : BaseApiService
    {
        private readonly ILogger<UrusanService>? _logger;

        public UrusanService(HttpClient httpClient, ILogger<UrusanService>? logger = null)
            : base(httpClient)
        {
            _logger = logger;
        }

        public async Task<MobileResponses<UrusanModel>> GetUrusanLists()
        {
            var urusanRasmiTask = GetListUrusanRasmi();
            var urusanTidakRasmiTask = GetListUrusanTidakRasmi();

            await Task.WhenAll(urusanRasmiTask, urusanTidakRasmiTask);

            var rasmi = urusanRasmiTask.Result.Data; //urusanRasmiTask.Result?.Data ?? new List<UrusanModel>();
            var tidakRasmi = urusanTidakRasmiTask.Result?.Data ?? new List<UrusanModel>();

            var combined = rasmi.Concat(tidakRasmi).ToList();

            return new MobileResponses<UrusanModel>
            {
                Data = combined,
                Success = urusanRasmiTask.Result?.Success ?? false,
                Messages = "Combined data"
            };
        }

        private async Task<MobileResponses<UrusanModel>> GetListUrusanTidakRasmi()
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

                var urusanTidakRasmiUrl = $"{BaseUrl}/alasantidakrasmi";

                var response = await HttpClient.GetAsync(urusanTidakRasmiUrl);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger?.LogWarning("Non-success status code: {StatusCode}", response.StatusCode);
                    return new();
                }

                var UrusanTidakRasmi = await response.Content.ReadFromJsonAsync<MobileResponses<UrusanModel>>();

                _logger?.LogInformation("API Response - Success: {Success}, Message: {Message}",
                UrusanTidakRasmi?.Success,
                UrusanTidakRasmi?.Messages);

                return UrusanTidakRasmi ?? new MobileResponses<UrusanModel>();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to load list of urusan");
                return new();
            }

        }

        private async Task<MobileResponses<UrusanModel>> GetListUrusanRasmi()
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

                var urusanRasmiUrl = $"{BaseUrl}/alasanrasmi";

                var response = await HttpClient.GetAsync(urusanRasmiUrl);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger?.LogWarning("Non-success status code: {StatusCode}", response.StatusCode);
                    return new();
                }

                var UrusanRasmi = await response.Content.ReadFromJsonAsync<MobileResponses<UrusanModel>>();

                _logger?.LogInformation("API Response - Success: {Success}, Message: {Message}",
                UrusanRasmi?.Success,
                UrusanRasmi?.Messages);

                return UrusanRasmi ?? new MobileResponses<UrusanModel>();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to load list of urusan");
                return new();
            }

        }
    }
}
