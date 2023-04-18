using Newtonsoft.Json;

namespace pinecone.Models;

/// <summary>
/// Create request model for creating new index.
/// </summary>
public class CreateRequest
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("dimension")]
    public int Dimension { get; set; }

    [JsonProperty("metric", NullValueHandling = NullValueHandling.Ignore)]
    public string Metric { get; set; }

    [JsonProperty("pods", NullValueHandling = NullValueHandling.Ignore)]
    public int? Pods { get; set; }

    [JsonProperty("replicas", NullValueHandling = NullValueHandling.Ignore)]
    public int? Replicas { get; set; }

    [JsonProperty("pod_Type", NullValueHandling = NullValueHandling.Ignore)]
    public string PodType { get; set; }

    [JsonProperty("metadata_Config", NullValueHandling = NullValueHandling.Ignore)]
    public Dictionary<string, object> MetadataConfig { get; set; }

    [JsonProperty("source_Collection", NullValueHandling = NullValueHandling.Ignore)]
    public string SourceCollection { get; set; }
}
