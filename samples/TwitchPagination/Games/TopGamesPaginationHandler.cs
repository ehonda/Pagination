using CursorBased;
using JetBrains.Annotations;

namespace TwitchPagination.Games;

/// <summary>
/// <b>📖 EXAMPLE:</b> Direct implementation of a pagination handler via the abstract base class
/// <see cref="PaginationHandler{TPaginationContext, TCursor, TItem}">CursorBased.PaginationHandler</see>.
/// </summary>
[UsedImplicitly]
public class TopGamesPaginationHandler : PaginationHandler<GetTopGamesResponse, Game>
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
