using EHonda.Pagination.CursorBased.Composite.CursorExtractors;

namespace TwitchPagination.Games.Composite;

/// <summary>
/// <b>📖 EXAMPLE:</b> Implementation of a cursor extractor to use in the
/// <see cref="TopGamesPaginationHandler">composite pagination handler example</see>.
/// </summary>
public class CursorExtractor : ICursorExtractor<GetTopGamesResponse>
{
    public Task<string?> ExtractCursorAsync(GetTopGamesResponse context)
        => Task.FromResult(context.Pagination.Cursor);
}
