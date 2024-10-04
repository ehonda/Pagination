using CursorBased.Composite;

namespace TwitchPagination.Games.Composite;

public class TopGamesPaginationHandler : PaginationHandler<GetTopGamesResponse, string, Game>
{
    public TopGamesPaginationHandler(GamesClient client)
        : base(new PageRetriever(client), new CursorExtractor(), new ItemExtractor())
    {
    }
}
