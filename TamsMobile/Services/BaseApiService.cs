using System.Net.Http.Headers;

namespace TamsMobile.Services;

public abstract class BaseApiService
{
    protected readonly HttpClient HttpClient;
    protected const string BaseUrl = "https://ptpacsisb.ddns.net:4433/TAMSApi";
    //protected const string BaseUrl = "https://192.168.0.240:8447/TAMSApi";

    protected BaseApiService(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    protected void SetAuthorizationToken(string token)
    {
        HttpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
    }

    protected void ClearAuthorizationToken()
    {
        HttpClient.DefaultRequestHeaders.Authorization = null;
    }
}