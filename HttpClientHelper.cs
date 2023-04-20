using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace pinecone
{
    public class HttpClientHelper
    {
        private readonly HttpClient _httpClient;

        public HttpClientHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse> SendRequestAsync<TResponse>(
            Uri requestUri,
            HttpMethod method,
            IDictionary<string, string> headers = null,
            CancellationToken cancellationToken = default)
        {
            return await SendRequestAsync<object,TResponse>(requestUri, method, null, headers, cancellationToken);
        }

        public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(
            Uri requestUri,
            HttpMethod method,
            TRequest content = default,
            IDictionary<string, string> headers = null,
            CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = requestUri,
            };

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            if (content != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(content))
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                };
            }

            using (var response = await _httpClient.SendAsync(request, cancellationToken))
            {
                if(response != null && response.IsSuccessStatusCode) { 
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(body);
                }
                else
                {
                    //To get Pinecone api errormessage
                    var body = await response.Content.ReadAsStringAsync();
                    throw new Exception(body);
                }
            }
        }
    }
}