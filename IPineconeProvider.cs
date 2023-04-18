using pinecone.Models;

namespace pinecone
{
    public interface IPineconeProvider
    {
        Task<string> CreateIndex(CreateRequest createRequest, CancellationToken cancellationToken = default);
        Task<string[]> GetIndex();
        Task<string> GetProjectName(CancellationToken cancellationToken = default);
        Task<QueryResponse> GetQuery(string indexName, QueryRequest queryRequest, CancellationToken cancellationToken = default);
        Task<UpsertResponse> Upsert(string indexName, UpsertRequest upsertRequest, CancellationToken cancellationToken = default);
    }
}