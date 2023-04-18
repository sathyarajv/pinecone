using Newtonsoft.Json;
namespace pinecone.Models;

public class Vector
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("values")]
    public List<double> Values { get; set; }

    [JsonProperty("sparseValues", NullValueHandling = NullValueHandling.Ignore)]
    public SparseValue? SparseValues { get; set; }

    [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
    public Dictionary<string, string>? Metadata { get; set; }
}
