using Newtonsoft.Json;
namespace pinecone.Models;

public class UpsertResponse
{
    [JsonProperty("upsertedCount", NullValueHandling = NullValueHandling.Ignore)]
    public int? UpsertedCount { get; set; }
}
