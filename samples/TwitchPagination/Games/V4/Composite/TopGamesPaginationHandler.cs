using CursorBased.V4.Composite;

namespace TwitchPagination.Games.V4.Composite;

public class TopGamesPaginationHandler : PaginationHandler<GetTopGamesResponse, string, Game>
{
    public TopGamesPaginationHandler(GamesClient client)
        : base(new PageRetriever(client), new CursorExtractor(), new ItemExtractor())
    {
    }
}
