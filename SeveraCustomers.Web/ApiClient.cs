using Models;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace SeveraCustomers.Web;

public class ApiClient(HttpClient httpClient, SeveraApiClient severaApiClient)
{
    public async Task<WeatherForecast[]> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<WeatherForecast>? forecasts = null;

        await foreach (var forecast in httpClient.GetFromJsonAsAsyncEnumerable<WeatherForecast>("/weatherforecast", cancellationToken))
        {
            if (forecasts?.Count >= maxItems)
            {
                break;
            }
            if (forecast is not null)
            {
                forecasts ??= [];
                forecasts.Add(forecast);
            }
        }

        return forecasts?.ToArray() ?? [];
    }

    public async Task<Customers[]> GetCustomers(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<Customers>? customers = null;

        await foreach (var forecast in httpClient.GetFromJsonAsAsyncEnumerable<Customers>("/customers", cancellationToken))
        {
            if (customers?.Count >= maxItems)
            {
                break;
            }
            if (forecast is not null)
            {
                customers ??= [];
                customers.Add(forecast);
            }
        }

        return customers?.ToArray() ?? [];
    }
    public async Task<Customers?> GetCustomer(int Id) => await httpClient.GetFromJsonAsync<Customers>($"/customer/{Id}");

    public async Task UpdateCustomer(int Id, Customers dto) => await httpClient.PutAsync($"/editcustomer/{Id}", new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json"));
    public async Task CreateCustomer(Customers dto) => await httpClient.PostAsync($"/createcustomer/", new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json"));
    public async Task<bool> SeveraCustomerExists(Guid id) => await httpClient.GetFromJsonAsync<bool>($"/severacustomerexists/{id}");
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
