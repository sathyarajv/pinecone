using Newtonsoft.Json;
namespace pinecone.Models;

public class QueryRequest
{
    [JsonProperty("namespace", NullValueHandling = NullValueHandling.Ignore)]
    public string Namespace { get; set; }

    [JsonProperty("topK")]
    public int TopK { get; set; }

    [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
    public Dictionary<string, object> Filter { get; set; }

    [JsonProperty("includeValues", NullValueHandling = NullValueHandling.Ignore)]
    public bool? IncludeValues { get; set; }

    [JsonProperty("includeMetadata", NullValueHandling = NullValueHandling.Ignore)]
    public bool? IncludeMetadata { get; set; }

    [JsonProperty("vector", NullValueHandling = NullValueHandling.Ignore)]
    public List<double> Vector { get; set; }

    [JsonProperty("sparseVector", NullValueHandling = NullValueHandling.Ignore)]
    public SparseValue? SparseVector { get; set; }

    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }
}
