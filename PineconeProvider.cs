using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pinecone.Models;
using System.Net.Http.Headers;

namespace pinecone;

/// <summary>
/// Pinecone Provider
/// </summary>
public class PineconeProvider : IPineconeProvider
{
    private readonly HttpClient _httpClient;

    private readonly string ApiKey;
    private readonly string Environment;

    /// <summary>
    /// Pinecone Provider
    /// </summary>
    /// <param name="apiKey">Api key
    /// </param>
    /// <param name="environment"></param>
    public PineconeProvider(string apiKey, string environment)
    {
        _httpClient = new HttpClient();
        ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        Environment = environment ?? throw new ArgumentNullException(nameof(environment));

    }

    /// <summary>
    /// Get all indexes of the configured environment.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    public async Task<string[]> ListIndexes(CancellationToken cancellationToken = default)
    {
        try
        {
            var httpClientHelper = new HttpClientHelper(_httpClient);
            var requestUri = new Uri($"https://controller.{Environment}.pinecone.io/databases");
            var headers = new Dictionary<string, string>
                            {
                                { "accept", "application/json" },
                                { "Api-Key", ApiKey }
                            };

            return await httpClientHelper.SendRequestAsync<string[]>(
                requestUri,
                HttpMethod.Get,
                headers,
                cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"PineconeClient: Error get index: {ex.Message}");
        }


    }

    /// <summary>
    /// Create an Index on PineCone with CreateRequest parameters.
    /// </summary>
    /// <param name="createRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    public async Task<string> CreateIndex(CreateRequest createRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var httpClientHelper = new HttpClientHelper(_httpClient);
            var requestUri = new Uri($"https://controller.{Environment}.pinecone.io/databases");
            var headers = new Dictionary<string, string>
                            {
                                { "accept", "application/json; charset=utf-8" },
                                { "Api-Key", ApiKey }
                            };

            return await httpClientHelper.SendRequestAsync<CreateRequest, string>(
                requestUri,
                HttpMethod.Post,
                createRequest,
                headers,
                cancellationToken);

        }
        catch (Exception ex)
        {
            throw new Exception($"PineconeClient: Error creating index: {ex.Message}");
        }

    }

    /// <summary>
    /// Query the index 
    /// </summary>
    /// <param name="indexName">Name of the index to query</param>
    /// <param name="projectName">Projectname of the index</param>
    /// <param name="queryRequest">parameters to query the index</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<QueryResponse> GetQuery(string indexName, string projectName, QueryRequest queryRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(indexName))
                throw new ArgumentNullException(nameof(indexName));

            if (string.IsNullOrWhiteSpace(projectName))
                projectName = await GetProjectName(cancellationToken);

            // Adjust the dimension of the input vector(s)
            int targetDimension = (await DescribeIndex(indexName))?.Database.Dimension ?? 0;
            queryRequest.Vector = PadVector(queryRequest.Vector, targetDimension);

            var httpClientHelper = new HttpClientHelper(_httpClient);
            var requestUri = new Uri($"https://{indexName}-{projectName}.svc.{Environment}.pinecone.io/query");
            var headers = new Dictionary<string, string>
                            {
                                { "accept", "application/json; charset=utf-8" },
                                { "Api-Key", ApiKey }
                            };

            return await httpClientHelper.SendRequestAsync<QueryRequest, QueryResponse>(
                requestUri,
                HttpMethod.Post,
                queryRequest,
                headers,
                cancellationToken);

        }
        catch (Exception ex)
        {
            throw new Exception($"PineconeClient: Error getting query: {ex.Message}");
        }
    }

    /// <summary>
    /// To Update or Insert vectors to the given indexname
    /// </summary>
    /// <param name="indexName">Name of the index to query</param>
    /// <param name="projectName">Projectname of the index</param>
    /// <param name="upsertRequest">Upsert request parameters including vector, namespace,..</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<UpsertResponse> Upsert(string indexName, string projectName, UpsertRequest upsertRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(projectName))
                projectName = await GetProjectName(cancellationToken);

            // Adjust the dimension of the input vector(s)
            int targetDimension = (await DescribeIndex(indexName))?.Database.Dimension ?? 0;
            foreach (Vector vector in upsertRequest.Vectors)
            {
                vector.Values = PadVector(vector.Values, targetDimension);
            }

            var httpClientHelper = new HttpClientHelper(_httpClient);
            var requestUri = new Uri($"https://{indexName}-{projectName}.svc.{Environment}.pinecone.io/vectors/upsert");
            var headers = new Dictionary<string, string>
                            {
                                { "accept", "application/json; charset=utf-8" },
                                { "Api-Key", ApiKey }
                            };

            return await httpClientHelper.SendRequestAsync<UpsertRequest, UpsertResponse>(
                requestUri,
                HttpMethod.Post,
                upsertRequest,
                headers,
                cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"PineconeClient: Error Upserting: {ex.Message}");
        }


    }

    /// <summary>
    /// Get details of given IndexName
    /// </summary>
    /// <param name="indexName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<DescribeIndexResponse> DescribeIndex(string indexName, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(indexName))
                throw new ArgumentNullException(nameof(indexName));

            var httpClientHelper = new HttpClientHelper(_httpClient);
            var requestUri = new Uri($"https://controller.{Environment}.pinecone.io/databases/{indexName}");
            var headers = new Dictionary<string, string>
                            {
                                { "accept", "application/json" },
                                { "Api-Key", ApiKey }
                            };

            return await httpClientHelper.SendRequestAsync<DescribeIndexResponse>(
                requestUri,
                HttpMethod.Get,
                headers,
                cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"PineconeClient: Error get index: {ex.Message}");
        }
    }

    /// <summary>
    /// Projectname used to access the index for Upsert and querying.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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
    /// <summary>
    /// To fill 0 on empty arrayindexes on vector to match the arraysize with dimesion used to create the Index. 
    /// </summary>
    /// <param name="inputVector"></param>
    /// <param name="targetDimension"></param>
    /// <returns></returns>
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
