using Newtonsoft.Json;
namespace pinecone.Models;

public class UpsertRequest
{
    [JsonProperty("vectors")]
    public List<Vector> Vectors { get; set; }

    [JsonProperty("namespace", NullValueHandling = NullValueHandling.Ignore)]
    public string Namespace { get; set; }
}
