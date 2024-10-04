using JetBrains.Annotations;
using Sequential.Composite.PageRetrievers;

namespace TwitchPagination.Games.Composite;

/// <summary>
/// <b>📖 EXAMPLE:</b> Implementation of a page retriever to use in the
/// <see cref="TopGamesPaginationHandler">composite pagination handler example</see>.
/// </summary>
[UsedImplicitly]
public class PageRetriever : IPageRetriever<GetTopGamesResponse>
{
    private readonly GamesClient _client;

    public PageRetriever(GamesClient client)
    {
        _client = client;
    }
    
    public Task<GetTopGamesResponse> GetAsync(GetTopGamesResponse? context,
        CancellationToken cancellationToken = default)
        => _client.GetTopGames(100, context?.Pagination.Cursor);
}
