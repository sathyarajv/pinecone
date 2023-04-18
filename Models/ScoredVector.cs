using Newtonsoft.Json;
namespace pinecone.Models;

public class ScoredVector
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)]
    public double? Score { get; set; }

    [JsonProperty("values", NullValueHandling = NullValueHandling.Ignore)]
    public List<double> Values { get; set; }

    [JsonProperty("sparseValues", NullValueHandling = NullValueHandling.Ignore)]
    public SparseValue SparseValues { get; set; }

    [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
    public Dictionary<string, object> Metadata { get; set; }
}
