using Models;
using Newtonsoft.Json;
using System.Text;

namespace SeveraCustomers.Web;

public class SeveraApiClient(HttpClient httpClient)
{
    private AccessBearerToken? _accessToken;
    private DateTime _sessionExpiry;
    private const string BaseUrl = "https://api.severa.visma.com/rest-api/v1.0";

    private record AccessBearerToken(string access_token, string access_token_expires_utc);

    private async Task<AccessBearerToken> GetSessionAsync()
    {
        if (_accessToken == null || DateTime.UtcNow >= _sessionExpiry)
        {
            _accessToken = await FetchSessionAsync();
            _sessionExpiry = DateTime.Parse(_accessToken.access_token_expires_utc);
        }
        return _accessToken;
    }

    private async Task<AccessBearerToken> FetchSessionAsync()
    {
        var requestBody = new
        {
            client_Id = "",
            client_Secret = "",
            scope = ""
        };

        var response = await httpClient.PostAsJsonAsync(BaseUrl+"/token", requestBody);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var session = JsonConvert.DeserializeObject<AccessBearerToken>(json);
        return session ?? throw new InvalidOperationException("Failed to fetch session.");
    }

    private async Task<T?> GetWithSessionAsync<T, V>(string url) where V : JsonConverter, new()
    {
        var session = await GetSessionAsync();
        var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl+url);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session.access_token);

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var resultData = JsonConvert.DeserializeObject<T>(json, new V());
        return resultData;
    }

    private async Task<T?> PostWithSessionAsync<T, V>(string url, T obj) where V : JsonConverter, new()
    {
        var session = await GetSessionAsync();
        var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + url);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session.access_token);
        request.Content = new StringContent(JsonConvert.SerializeObject(obj, new V()), Encoding.UTF8, "application/json");
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var resultData = JsonConvert.DeserializeObject<T>(json, new V());
        return resultData;
    }

    private async Task DeleteWithSessionAsync(string url)
    {
        var session = await GetSessionAsync();
        var request = new HttpRequestMessage(HttpMethod.Delete, BaseUrl + url);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session.access_token);
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task<Customers[]?> GetSeveraCustomer() => await GetWithSessionAsync<Customers[], CustomersJsonConverter>($"/customers");
    public async Task<Guid> PostToSevera(Customers dto)
    {
        var response = await PostWithSessionAsync<Customers, CustomersJsonConverter>($"/customers", dto);
        return response.Guid.Value;
    }
    public async Task DeleteSeveraCustomer(Guid guid) => await DeleteWithSessionAsync($"/customers/{guid}");
}