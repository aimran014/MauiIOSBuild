using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TamsMobile.Models;

namespace TamsMobile.Services
{
    public class GPSService : BaseApiService
    {
        private readonly ILogger<GPSService>? _logger;
        public GPSService(HttpClient httpClient, ILogger<GPSService>? logger = null)
        : base(httpClient)
        {
            _logger = logger;
        }

        public async Task<MobileResponse<GPSModel>> FetchCurrentLocationAsync()
        {
            //return await GetGPSDataAsync();

            // This function only work for mobile device with GPS hardware without using token map. For desktop or emulator, it will return error "GPS not supported".
            return await GetCurrentLocationAsync();
        }

        private async Task<MobileResponse<GPSModel>> GetGPSDataAsync()
        {
            try
            {
                MobileResponse<GPSModel> response = new MobileResponse<GPSModel>();
                var location = await Geolocation.GetLastKnownLocationAsync()
                       ?? await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    return await GetGPSDataWithPermissionAsync(location.Latitude, location.Longitude);
                }
                else
                {
                    response.Success = false;
                    response.Messages = "Lokasi tidak dijumpai";
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error fetching GPS data.");
                return new MobileResponse<GPSModel> { Success = false, Messages = $"Error: {ex.Message}" };
            }
        }

        private async Task<MobileResponse<GPSModel>> GetGPSDataWithPermissionAsync(double lat, double lon)
        {
            try
            {
                var url = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={lat}&lon={lon}&zoom=18&addressdetails=1";

                var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("TamsMobileApp/1.0");
                var result = await client.GetFromJsonAsync<NominatimResponse>(url);

                var a = result?.address;

                if (a == null)
                    return new MobileResponse<GPSModel> { Success = false, Messages = "Alamat tidak dijumpai" };

                var parts = new List<string>();

                if (!string.IsNullOrWhiteSpace(a.building)) parts.Add(a.building);
                if (!string.IsNullOrWhiteSpace(a.road)) parts.Add(a.road);
                if (!string.IsNullOrWhiteSpace(a.neighbourhood)) parts.Add(a.neighbourhood);
                if (!string.IsNullOrWhiteSpace(a.suburb)) parts.Add(a.suburb);
                if (!string.IsNullOrWhiteSpace(a.city)) parts.Add(a.city);
                if (!string.IsNullOrWhiteSpace(a.state)) parts.Add(a.state);
                if (!string.IsNullOrWhiteSpace(a.postcode)) parts.Add(a.postcode);
                if (!string.IsNullOrWhiteSpace(a.country)) parts.Add(a.country);

                return new MobileResponse<GPSModel>
                {
                    Success = true,
                    Data = new GPSModel
                    {
                        Latitude = lat.ToString(),
                        Longitude = lon.ToString(),
                        Address_1 = string.Join(", ", parts.Take(2)),
                        Address_2 = string.Join(", ", parts.Skip(2).Take(2)),
                        City = a.city,
                        State = a.state,
                        PostalCode = a.postcode,
                        Country = a.country
                    },
                    Messages = "Alamat berjaya dijumpai."
                };

                //var response = await client.GetFromJsonAsync<NominatimResponse>(url);

                //return response?.display_name ?? "Alamat tidak dijumpai";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error checking location permissions.");
                return new MobileResponse<GPSModel> { Success = false, Messages = $"Error: {ex.Message}" };
            }
        }

        private async Task<MobileResponse<GPSModel>> GetCurrentLocationAsync()
        {
            var response = new MobileResponse<GPSModel>();

            bool hasPermission = await CheckLocationPermission();

            if (!hasPermission)
            {
                return new MobileResponse<GPSModel>
                {
                    Success = false,
                    Messages = "Location permission denied.Please allow location access in your device settings."
                };
            }

            try
            {
                // 1. Check permission
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }

                if (status != PermissionStatus.Granted)
                {
                    response.Success = false;
                    response.Messages = "Location permission denied";
                    return response;
                }

                // 2. Get GPS coordinate
                var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
                var location = await Geolocation.Default.GetLocationAsync(request);

                if (location == null)
                {
                    response.Success = false;
                    response.Messages = "Unable to get location";
                    return response;
                }

                // 3. Reverse Geocoding (Lat/Lon → Address)
                var placemarks = await Geocoding.Default.GetPlacemarksAsync(location.Latitude, location.Longitude);
                var place = placemarks?.FirstOrDefault();

                // 4. Map to your model
                var gps = new GPSModel
                {
                    Latitude = location.Latitude.ToString(),
                    Longitude = location.Longitude.ToString(),
                    Address_1 = place?.Thoroughfare,          // Street
                    Address_2 = place?.SubThoroughfare,       // Building / number
                    City = place?.Locality,
                    State = place?.AdminArea,
                    PostalCode = place?.PostalCode,
                    Country = place?.CountryName
                };

                response.Success = true;
                response.Data = gps;
                response.Messages = "Success";
            }
            catch (FeatureNotEnabledException)
            {
                response.Success = false;
                response.Messages = "GPS is turned off";
            }
            catch (FeatureNotSupportedException)
            {
                response.Success = false;
                response.Messages = "GPS not supported";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Messages = ex.Message;
            }

            return response;
        }

        private async Task<bool> CheckLocationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            return status == PermissionStatus.Granted;
        }
    }
}
