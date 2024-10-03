using Sequential.V2.Composite;
using Sequential.V2.Composite.PageRetrievers;

namespace TwitchPagination.Games.V4.Composite;

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
