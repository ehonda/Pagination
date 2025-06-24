using EHonda.Pagination.CursorBased;
using TestUtilities.Games;

namespace CursorBased.TUnit.Tests.TestUtilities;

public class GamesCursorPaginationHandler : PaginationHandler<Page, string>
{
    private readonly GamesClient _client;
    private readonly int _pageSize;

    public GamesCursorPaginationHandler(GamesClient client, int pageSize)
    {
        _client = client;
        _pageSize = pageSize;
    }

    protected override async Task<Page> GetPageAsync(Page? previousPage,
        CancellationToken cancellationToken = default)
    {
        var clientPage = await _client.GetGamesByCursor(previousPage?.Pagination.Cursor, _pageSize);
        
        return new(clientPage.Games, new(clientPage.Cursor));
    }

    protected override Task<string?> ExtractCursorAsync(Page context)
    {
        return Task.FromResult(context.Pagination.Cursor);
    }

    protected override IAsyncEnumerable<string> ExtractItemsAsync(Page page,
        CancellationToken cancellationToken = default)
    {
        return page.Data.ToAsyncEnumerable();
    }
}
