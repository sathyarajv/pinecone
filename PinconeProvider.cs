using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pinecone.Models;
using System.Net.Http.Headers;

namespace pinecone;

public class PinconeProvider : IPineconeProvider
{
    private readonly HttpClient _httpClient;

    public string ApiKey { get; set; }
    public string Environment { get; set; }
    public string ProjectName { get; set; }

    public PinconeProvider()
    {
        _httpClient = new HttpClient();

    }
    public async Task<string[]> GetIndex()
    {
        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://controller.{Environment}.pinecone.io/databases"),
                Headers =
                        {
                            { "accept", "application/json; charset=utf-8" },
                            { "Api-Key",  ApiKey},
                        },
            };
            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<string[]>(body);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"PineconeClient: Error get index: {ex.Message}");
        }


    }

    public async Task<string> CreateIndex(CreateRequest createRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://controller.{Environment}.pinecone.io/databases"),
                Headers =
                        {
                            { "accept", "application/json; charset=utf-8" },
                            { "Api-Key",  ApiKey},
                        },
                Content = new StringContent(JsonConvert.SerializeObject(createRequest))
                {
                    Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                }
            };
            using (var response = await _httpClient.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return body;
            }

        }
        catch (Exception ex)
        {
            throw new Exception($"PineconeClient: Error creating index: {ex.Message}");
        }

    }

    public async Task<QueryResponse> GetQuery(string indexName, QueryRequest queryRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://{indexName}-{ProjectName}.svc.{Environment}.pinecone.io/query"),
                Headers =
                        {
                            { "accept", "application/json" },
                            { "Api-Key", ApiKey },
                        },
                Content = new StringContent(JsonConvert.SerializeObject(queryRequest))
                {
                    Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
                }
            };
            using (var response = await _httpClient.SendAsync(request,cancellationToken))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<QueryResponse>(body);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"PineconeClient: Error getting query: {ex.Message}");
        }
    }

    public async Task<UpsertResponse> Upsert(string indexName, UpsertRequest upsertRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            int targetDimension = 1536;

            // Adjust the dimension of the input vector(s)
            for (int i = 0; i < upsertRequest.Vectors.Count; i++)
            {
                upsertRequest.Vectors[i].Values = PadVector(upsertRequest.Vectors[i].Values, targetDimension);
            }
             var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://{indexName}-{ProjectName}.svc.{Environment}.pinecone.io/vectors/upsert"),
                Headers =
            {
                { "accept", "application/json" },
                { "Api-Key", ApiKey },
            },
                Content = new StringContent(JsonConvert.SerializeObject(upsertRequest))
                {
                    Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
                }
            };
            using (var response = await _httpClient.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UpsertResponse>(body);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"PineconeClient: Error Upserting: {ex.Message}");
        }


    }

    public async Task<string> GetProjectName(CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://controller.{Environment}.pinecone.io/actions/whoami"),
                Headers =
                        {
                            { "accept", "application/json" },
                            { "Api-Key", ApiKey },
                        },

            };
            using (var response = await _httpClient.SendAsync(request, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(body);
                var _projectName = jsonResponse["project_name"].ToString();
                return _projectName;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"PineconeClient: Error getting project name: {ex.Message}");
        }
    }

    #region HelperMethods
    private List<double> PadVector(List<double> inputVector, int targetDimension)
    {
        int inputDimension = inputVector.Count;

        if (inputDimension == targetDimension)
        {
            return inputVector;
        }
        else if (inputDimension > targetDimension)
        {
            return inputVector.Take(targetDimension).ToList();
        }
        else
        {
            List<double> paddedVector = new List<double>(inputVector);
            for (int i = inputDimension; i < targetDimension; i++)
            {
                paddedVector.Add(0);
            }
            return paddedVector;
        }
    }
    #endregion
}
