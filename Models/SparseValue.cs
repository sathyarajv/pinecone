using Newtonsoft.Json;
namespace pinecone.Models;

public class SparseValue
{
    public long[] Indices { get; set; }
    public float[] Values { get; set; }
}