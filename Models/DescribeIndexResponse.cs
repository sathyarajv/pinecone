using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pinecone.Models
{
    public class DescribeIndexResponse
    {
        public Database Database { get; set; }
        public Status Status { get; set; }
    }

    public class Database
    {
        public string Name { get; set; }
        public string Metric { get; set; }
        public int Dimension { get; set; }
        public int Replicas { get; set; }
        public int Shards { get; set; }
        public int Pods { get; set; }
        public string PodType { get; set; }
        public MetadataConfig MetadataConfig { get; set; }
    }

    public class MetadataConfig
    {
        public List<string> Indexed { get; set; }
    }

    public class Status
    {
        public List<object> Waiting { get; set; }
        public List<object> Crashed { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string State { get; set; }
        public bool Ready { get; set; }
    }
}
