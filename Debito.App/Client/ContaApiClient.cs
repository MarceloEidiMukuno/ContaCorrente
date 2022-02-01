using System.Net;

namespace ContaCorrente.ApiDebito.Clients
{
    public static class ContaApiClient
    {

        public static async Task<bool> GetContaAsync(string Agencia, string Conta)
        {
            HttpClientHandler handler = new HttpClientHandler();

            // Set custom server validation callback
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };

            HttpClient client = new HttpClient(handler);

            HttpResponseMessage response = await client.GetAsync($"https://contas.azurewebsites.net/v1/conta/{Agencia}/{Conta}");

            return response.IsSuccessStatusCode;

        }
    }
}