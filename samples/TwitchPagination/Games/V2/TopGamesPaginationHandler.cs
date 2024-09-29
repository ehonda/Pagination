namespace TwitchPagination.Games.V2;

// This looks very clean, we only need to implement two trivial methods
public class TopGamesPaginationHandler : CursorBased.V2.PaginationHandler<
    PaginationContext, string, Game>
{
    private readonly GamesClient _client;

    public TopGamesPaginationHandler(GamesClient client)
    {
        _client = client;
    }
    
    protected override async Task<PaginationContext> GetPageAsync(PaginationContext? context,
        CancellationToken cancellationToken = default)
        => new(await _client.GetTopGames(100, context?.Cursor));

    protected override IAsyncEnumerable<Game> ExtractItemsAsync(PaginationContext context,
        CancellationToken cancellationToken = default)
        => context.Games.ToAsyncEnumerable();
}
