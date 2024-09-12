using Sequential.Composite;

namespace CursorBased.Http;

public abstract class PageRetriever<TTransformedPage, TCursor>
    : IPageRetriever<PaginationContext<TTransformedPage, TCursor>, HttpResponseMessage>
{
    protected HttpClient HttpClient { get; }
    
    public PageRetriever(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    public abstract Task<HttpResponseMessage> GetAsync(
        PaginationContext<TTransformedPage, TCursor>? context,
        CancellationToken cancellationToken = default);
}
