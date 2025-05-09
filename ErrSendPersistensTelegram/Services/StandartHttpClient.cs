using ErrSendApplication.Common.Configs;
using ErrSendApplication.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace ErrSendPersistensTelegram.Services
{
    public class StandartHttpClient : IHttpClientWr
    {
        private readonly HttpClient httpClient;
        private BaseAddressCfg baseAddressCfg;
        private bool disposed = false;

        public StandartHttpClient(HttpClient httpClient, IOptions<BaseAddressCfg> baseAddressCfg)
        {
            this.baseAddressCfg = baseAddressCfg.Value;
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.httpClient.Timeout = TimeSpan.FromMinutes(180); // Встановити тайм-аут на 120 хвилин ПОКИ КОСТИЛЬ
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content, string? token = null)
        {
            if (token is not null)
            {
                // Видалити існуючий заголовок Authorization, якщо він існує
                if (httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    httpClient.DefaultRequestHeaders.Remove("Authorization");
                }

                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await httpClient.PostAsync(url, content);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    httpClient?.Dispose();
                }
                disposed = true;
            }
        }
    }
}
