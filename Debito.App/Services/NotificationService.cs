using System.Text;
using System.Text.Json;

namespace ContaCorrente.ApiDebito.Services
{
    public class NotificationService
    {

        public async Task NotifyAsync<T>(string Url, T data)
        {
            var httpClient = new HttpClient();
            var notify = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                IgnoreNullValues = true
            });
            var content = new StringContent(notify, Encoding.UTF8, "application/json");
            var httpResponse = await httpClient.PostAsync(Url, content);
        }
    }
}