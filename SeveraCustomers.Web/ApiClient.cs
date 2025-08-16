using Models;

namespace SeveraCustomers.Web;

public class ApiClient(HttpClient httpClient)
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
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
