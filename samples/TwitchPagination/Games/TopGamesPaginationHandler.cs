using CursorBased;

namespace TwitchPagination.Games;

public class TopGamesPaginationHandler : CursorBased.PaginationHandler<
    GetTopGamesResponse,
    GetTopGamesResponse,
    string,
    Game>
{
    private readonly GamesClient _client;

    public TopGamesPaginationHandler(GamesClient client)
    {
        _client = client;
    }
    
    protected override Task<GetTopGamesResponse> GetPageAsync(PaginationContext<GetTopGamesResponse, string>? context,
        CancellationToken cancellationToken = default)
        // This is nice
        // TODO: Make `first` configurable
        => _client.GetTopGames(100, context?.Cursor);

    protected override Task<PaginationContext<GetTopGamesResponse, string>> ExtractContextAsync(
        GetTopGamesResponse page, CancellationToken cancellationToken = default)
        // Awkward
        => Task.FromResult(new PaginationContext<GetTopGamesResponse, string>(page, page.Pagination.Cursor));

    protected override IAsyncEnumerable<Game> ExtractItemsAsync(PaginationContext<GetTopGamesResponse, string> context,
        CancellationToken cancellationToken = default)
        // Also nice
        => context.Page.Data.ToAsyncEnumerable();
}
