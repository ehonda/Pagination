using EHonda.Pagination.CursorBased.Composite;
using JetBrains.Annotations;

namespace TwitchPagination.Games.Composite;

/// <summary>
/// <b>📖 EXAMPLE:</b> Direct implementation of a pagination handler via the abstract base class
/// <see cref="PaginationHandler{TPaginationContext, TCursor, TItem}">CursorBased.Composite.PaginationHandler</see>.
/// </summary>
[UsedImplicitly]
public class TopGamesPaginationHandler : PaginationHandler<GetTopGamesResponse, Game>
{
    public TopGamesPaginationHandler(GamesClient client)
        : base(new PageRetriever(client), new CursorExtractor(), new ItemExtractor())
    {
    }
}
