using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.JSInterop;

namespace ForestSchedule.Client.Services;

public class ApiClient(HttpClient http, IJSRuntime jsRuntime)
{
    private async Task SetBearerTokenAsync()
    {
        try
        {
            var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (!string.IsNullOrEmpty(token))
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                http.DefaultRequestHeaders.Authorization = null;
            }
        }
        catch {}
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        await SetBearerTokenAsync();
        var response = await http.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Помилка: {response.StatusCode}. Деталі: {error}");
        }

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<TResponse?> PostAsync<TResponse, TRequest>(string url, TRequest data)
    {
        await SetBearerTokenAsync();
        var response = await http.PostAsJsonAsync(url, data);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Помилка: {response.StatusCode}. Деталі: {error}");
        }

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task DeleteAsync(string url)
    {
        await SetBearerTokenAsync();
        var response = await http.DeleteAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Помилка видалення: {response.StatusCode}. Деталі: {error}");
        }
    }
}