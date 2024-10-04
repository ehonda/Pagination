using CursorBased;

namespace TwitchPagination.Games;

// This looks very clean as well, we only need to implement two trivial methods, we drop the
// implementation of a pagination context and new up a cursor extractor instead
public class TopGamesPaginationHandler : PaginationHandler<
    GetTopGamesResponse, string, Game>
{
    private readonly GamesClient _client;

    public TopGamesPaginationHandler(GamesClient client)
    {
        _client = client;
    }

    protected override async Task<GetTopGamesResponse> GetPageAsync(GetTopGamesResponse? context,
        CancellationToken cancellationToken = default)
        => await _client.GetTopGames(100, context?.Pagination.Cursor);

    protected override IAsyncEnumerable<Game> ExtractItemsAsync(GetTopGamesResponse context,
        CancellationToken cancellationToken = default)
        => context.Data.ToAsyncEnumerable();

    protected override Task<string?> ExtractCursorAsync(GetTopGamesResponse context)
        => Task.FromResult(context.Pagination.Cursor);
}
