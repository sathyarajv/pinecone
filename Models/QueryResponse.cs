using Newtonsoft.Json;
namespace pinecone.Models;

public class QueryResponse
{
    [JsonProperty("matches", NullValueHandling = NullValueHandling.Ignore)]
    public List<ScoredVector> Matches { get; set; }

    [JsonProperty("namespace", NullValueHandling = NullValueHandling.Ignore)]
    public string Namespace { get; set; }
}
