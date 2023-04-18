using pinecone.Models;

namespace pinecone
{
    public interface IPineconeProvider
    {
        public string ApiKey { get; set; }
        public string Environment { get; set; }
        public string ProjectName { get; set; }
        Task<string> CreateIndex(CreateRequest createRequest, CancellationToken cancellationToken = default);
        Task<string[]> GetIndex();
        Task<string> GetProjectName(CancellationToken cancellationToken = default);
        Task<QueryResponse> GetQuery(string indexName, QueryRequest queryRequest, CancellationToken cancellationToken = default);
        Task<UpsertResponse> Upsert(string indexName, UpsertRequest upsertRequest, CancellationToken cancellationToken = default);
    }
}