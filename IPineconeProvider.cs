using pinecone.Models;

namespace pinecone
{
    /// <summary>
    /// Pinecone Provider
    /// </summary>
    public interface IPineconeProvider
    {
        /// <summary>
        /// Create an Index on PineCone with CreateRequest parameters.
        /// </summary>
        /// <param name="createRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> CreateIndex(CreateRequest createRequest, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all indexes of the configured environment.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string[]> ListIndexes(CancellationToken cancellationToken = default);
        /// <summary>
        /// Projectname used to access the index for Upsert and querying.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetProjectName(CancellationToken cancellationToken = default);

        /// <summary>
        /// Query the index 
        /// </summary>
        /// <param name="indexName">Name of the index to query</param>
        /// <param name="projectName">Projectname of the index</param>
        /// <param name="queryRequest">parameters to query the index</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<QueryResponse> GetQuery(string indexName, string projectName, QueryRequest queryRequest, CancellationToken cancellationToken = default);

        /// <summary>
        /// To Update or Insert vectors to the given indexname
        /// </summary>
        /// <param name="indexName">Name of the index to query</param>
        /// <param name="projectName">Projectname of the index</param>
        /// <param name="upsertRequest">Upsert request parameters including vector, namespace,..</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UpsertResponse> Upsert(string indexName, string projectName, UpsertRequest upsertRequest, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get details of given IndexName
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DescribeIndexResponse> DescribeIndex(string indexName, CancellationToken cancellationToken = default);
    }
}