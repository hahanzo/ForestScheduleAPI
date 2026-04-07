using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ForestSchedule.Client.Services
{
    public class ApiClient(HttpClient http, IJSRuntime jsRuntime)
    {
        public async Task SetBearerTokenAsync()
        {
            var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (!string.IsNullOrEmpty(token))
            {
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            await SetBearerTokenAsync();
            var response = await http.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Exception API: {response.StatusCode}");

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string url, T data)
        {
            await SetBearerTokenAsync();
            return await http.PostAsJsonAsync(url, data);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            await SetBearerTokenAsync();
            return await http.DeleteAsync(url);
        }
    }
}
