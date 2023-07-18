using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DotnetLibraries
{
    public class WebApiClient : IDisposable
    {
        private readonly HttpClient _client;

        public Uri BaseAddress
        {
            get { return _client.BaseAddress; }
            set { _client.BaseAddress = value; }
        }

        public TimeSpan Timeout
        {
            get { return _client.Timeout; }
            set { _client.Timeout = value; }
        }

        public WebApiClient()
        {
            _client = new HttpClient();
            _client.Timeout = TimeSpan.FromMinutes(10);
        }

        public WebApiClient(HttpMessageHandler handler = null)
        {
            _client = handler == null ? new HttpClient() : new HttpClient(handler);
            _client.Timeout = TimeSpan.FromMinutes(10);
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await SendAsync(HttpMethod.Get, requestUri, null);
        }
        //public Task<HttpResponseMessage> GetAsync(string requestUri)
        //{
        //    return SendAsync(HttpMethod.Get, requestUri, null);
        //}

        public async Task<HttpResponseMessage> PostAsync(string requestUri, string text, string mediaType = "application/json")
        {
            using (var content = new StringContent(text, Encoding.UTF8, mediaType))
            {
                return await SendAsync(HttpMethod.Post, requestUri, content);
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            using (var content = new FormUrlEncodedContent(parameters))
            {
                return await SendAsync(HttpMethod.Post, requestUri, content);
            }
        }

        public async Task<string> TryPostJsonAsync(string requestUri, string jsonText)
        {
            using (var content = new StringContent(jsonText, Encoding.UTF8, "application/json"))
            {
                var response = await SendAsync(HttpMethod.Post, requestUri, content);
                return await response.Content.ReadAsStringAsync();
            }
        }

        private async Task<HttpResponseMessage> SendAsync(HttpMethod httpMethod, string requestUri, HttpContent content)
        {
            using (var request = new HttpRequestMessage(httpMethod, new Uri(BaseAddress, requestUri)))
            {
                if (content != null)
                {
                    request.Content = content;
                }

                return await _client.SendAsync(request);
            }
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
