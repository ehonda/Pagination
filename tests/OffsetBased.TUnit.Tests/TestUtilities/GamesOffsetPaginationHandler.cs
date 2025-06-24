using EHonda.Pagination.OffsetBased;
using TestUtilities.Games;

namespace OffsetBased.TUnit.Tests.TestUtilities;

public class GamesOffsetPaginationHandler : PaginationHandler<Page, string>
{
    private readonly GamesClient _client;
    private readonly int _pageSize;

    public GamesOffsetPaginationHandler(GamesClient client, int pageSize)
    {
        _client = client;
        _pageSize = pageSize;
    }

    protected override async Task<Page> GetPageAsync(Page? previousPage,
        CancellationToken cancellationToken = default)
    {
        var offset = previousPage != null ? previousPage.Pagination.Offset + previousPage.Items.Count : 0;
        var items = await _client.GetGamesByOffset(offset, _pageSize);
        return new(items, new(offset, _client.TotalCount));
    }

    protected override Task<OffsetState<int>> ExtractOffsetStateAsync(Page context,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new OffsetState<int>(context.Pagination.Offset, context.Pagination.Total));
    }

    protected override IAsyncEnumerable<string> ExtractItemsAsync(Page page,
        CancellationToken cancellationToken = default)
    {
        return page.Items.ToAsyncEnumerable();
    }
}
