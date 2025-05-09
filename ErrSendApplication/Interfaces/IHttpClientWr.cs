namespace ErrSendApplication.Interfaces
{
    public interface IHttpClientWr :IDisposable
    {
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content, string? token = null);
    }
}
