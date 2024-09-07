using Core;
using Sequential;

namespace DirectImplementation;

public class GitHubOriginalPaginationHandler<TItem> : OriginalPaginationHandler<HttpResponseMessage, HttpResponseMessage, TItem>
{
    private readonly HttpClient _httpClient;
    private readonly Func<HttpResponseMessage, CancellationToken, IAsyncEnumerable<TItem>> _itemExtractor;

    public GitHubOriginalPaginationHandler(
        HttpClient httpClient,
        Func<HttpResponseMessage, CancellationToken, IAsyncEnumerable<TItem>> itemExtractor)
    {
        _httpClient = httpClient;
        _itemExtractor = itemExtractor;
    }

    protected override Task<HttpResponseMessage> GetFirstPageAsync(CancellationToken cancellationToken = default)
        => _httpClient.GetAsync((Uri?)null, cancellationToken);

    protected override Task<HttpResponseMessage> TransformPageAsync(
        HttpResponseMessage page,
        CancellationToken cancellationToken = default)
        => Task.FromResult(page);

    protected override Task<bool> NextPageExistsAsync(
        HttpResponseMessage currentPage,
        CancellationToken cancellationToken = default)
    {
        // Handle link headers, see: https://docs.github.com/en/rest/using-the-rest-api/using-pagination-in-the-rest-api?apiVersion=2022-11-28#using-link-headers
        return Task.FromResult(true);
    }

    protected override Task<HttpResponseMessage> GetNextPageAsync(
        HttpResponseMessage currentPage,
        CancellationToken cancellationToken = default)
    {
        // Handle link headers, see: https://docs.github.com/en/rest/using-the-rest-api/using-pagination-in-the-rest-api?apiVersion=2022-11-28#using-link-headers
        return Task.FromResult(currentPage);
    }

    protected override IAsyncEnumerable<TItem> ExtractItemsAsync(
        HttpResponseMessage page,
        CancellationToken cancellationToken = default)
        => _itemExtractor(page, cancellationToken);
}
