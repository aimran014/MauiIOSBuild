using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TamsMobile.Models;
using static TamsMobile.Models.KemaskiniAttendanceModel;

namespace TamsMobile.Services
{
    public class KemaskiniAttendanceService : BaseApiService
    {
        private readonly ILogger<KemaskiniAttendanceService>? _logger;

        public KemaskiniAttendanceService(HttpClient httpClient, ILogger<KemaskiniAttendanceService>? logger = null)
            : base(httpClient)
        {
            _logger = logger;
        }

        public async Task<MobileResponse<KemaskiniDetails>> LoadKemaskiniDetails(int AttendanceId)
        {
            return await GetKemaskiniDetails(AttendanceId);
        }


        public async Task<MobileResponse<string>> ProcessKemaskiniAttendance(KemaskiniAttendanceRequestModel requestModel)
        {
            //return await SubmitAddKemaskiniAttendance(requestModel);

            if (requestModel?.updateKemasKini?.attendance_id != null)
            {
                return await SubmitUpdateKemaskiniAttendance(requestModel.updateKemasKini);
            }
            else if (requestModel?.addKemasKini?.attendance_id != null)
            {
                return await SubmitAddKemaskiniAttendance(requestModel.addKemasKini);
            }
            else if (requestModel?.pelulusKemaskini?.attendance_id != null)
            {
                return await SubmitPelulusKemaskiniAttendance(requestModel.pelulusKemaskini);
            }
            else
            {
                return await SubmitDeleteKemaskiniAttendance(requestModel?.deleteKemasKiniId ?? string.Empty);
            }

        }

        private async Task<MobileResponse<KemaskiniDetails>> GetKemaskiniDetails(int AttendanceId)
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
                var attendanceUrl = $"{BaseUrl}/detailskemaskinikedatangan?AttendanceId={AttendanceId}";
                var response = await HttpClient.GetAsync(attendanceUrl);
                var content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    _logger?.LogWarning("Non-success status code: {StatusCode}", response.StatusCode);
                    return new();
                }
                var result = await response.Content.ReadFromJsonAsync<MobileResponse<KemaskiniDetails>>();
                _logger?.LogInformation("API Response - Success: {Success}, Message: {Message}",
                    result?.Success, result?.Messages);
                return result ?? new MobileResponse<KemaskiniDetails>();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to load kemaskini details for ID: {Id}", AttendanceId);
                return new();
            }

        }

        private async Task<MobileResponse<string>> SubmitAddKemaskiniAttendance(KemaskiniAttendanceAddModel requestModel)
        {
            try
            {
                // GET TOKEN
                var token = await SecureStorage.GetAsync("auth_token");
                if (string.IsNullOrEmpty(token))
                {
                    _logger?.LogWarning("No auth token found in SecureStorage.");
                    return new MobileResponse<string>
                    {
                        Success = false,
                        Messages = "Authentication required."
                    };
                }

                // SET TOKEN
                SetAuthorizationToken(token);

                var url = $"{BaseUrl}/addkemaskinikedatangan";

                // ✅ Send request
                var response = await HttpClient.PostAsJsonAsync(url, requestModel);

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
                _logger?.LogError(ex, "Error submitting kemaskini attendance");

                return new MobileResponse<string>
                {
                    Success = false,
                    Messages = $"Exception: {ex.Message}"
                };
            }
        }

        private async Task<MobileResponse<string>> SubmitUpdateKemaskiniAttendance(KemaskiniAttendanceUpdateModel requestModel)
        {
            try
            {
                // GET TOKEN
                var token = await SecureStorage.GetAsync("auth_token");
                if (string.IsNullOrEmpty(token))
                {
                    _logger?.LogWarning("No auth token found in SecureStorage.");
                    return new MobileResponse<string>
                    {
                        Success = false,
                        Messages = "Authentication required."
                    };
                }

                // SET TOKEN
                SetAuthorizationToken(token);

                var url = $"{BaseUrl}/updatekemaskinikedatangan";

                // ✅ Send request
                var response = await HttpClient.PostAsJsonAsync(url, requestModel);

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
                _logger?.LogError(ex, "Error submitting kemaskini attendance");

                return new MobileResponse<string>
                {
                    Success = false,
                    Messages = $"Exception: {ex.Message}"
                };
            }

        }

        private async Task<MobileResponse<string>> SubmitPelulusKemaskiniAttendance(KemaskiniAttendancePelulusModel requestModel)
        {
            try
            {
                // GET TOKEN
                var token = await SecureStorage.GetAsync("auth_token");
                if (string.IsNullOrEmpty(token))
                {
                    _logger?.LogWarning("No auth token found in SecureStorage.");
                    return new MobileResponse<string>
                    {
                        Success = false,
                        Messages = "Authentication required."
                    };
                }

                // SET TOKEN
                SetAuthorizationToken(token);

                var url = $"{BaseUrl}/peluluskemaskinikedatangan";

                // ✅ Send request
                var response = await HttpClient.PostAsJsonAsync(url, requestModel);

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
                _logger?.LogError(ex, "Error submitting kemaskini attendance");

                return new MobileResponse<string>
                {
                    Success = false,
                    Messages = $"Exception: {ex.Message}"
                };
            }

        }

        private async Task<MobileResponse<string>> SubmitDeleteKemaskiniAttendance(string requestModel)
        {
            try
            {
                // GET TOKEN
                var token = await SecureStorage.GetAsync("auth_token");
                if (string.IsNullOrEmpty(token))
                {
                    _logger?.LogWarning("No auth token found in SecureStorage.");
                    return new MobileResponse<string>
                    {
                        Success = false,
                        Messages = "Authentication required."
                    };
                }

                // SET TOKEN
                SetAuthorizationToken(token);

                var url = $"{BaseUrl}/deletekemaskinikedatangan";

                // ✅ Send request
                var response = await HttpClient.PostAsJsonAsync(url, requestModel);

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
                _logger?.LogError(ex, "Error submitting kemaskini attendance");

                return new MobileResponse<string>
                {
                    Success = false,
                    Messages = $"Exception: {ex.Message}"
                };
            }

        }
    }
}
