using CursorBased.V4.Composite;
using CursorBased.V4.Composite.CursorExtractors;

namespace TwitchPagination.Games.V4;

public class CursorExtractor : ICursorExtractor<GetTopGamesResponse, string>
{
    public Task<string?> ExtractCursorAsync(GetTopGamesResponse context)
        => Task.FromResult(context.Pagination.Cursor);
}
