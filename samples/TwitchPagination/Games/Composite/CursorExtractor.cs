using CursorBased.Composite.CursorExtractors;

namespace TwitchPagination.Games.Composite;

public class CursorExtractor : ICursorExtractor<GetTopGamesResponse, string>
{
    public Task<string?> ExtractCursorAsync(GetTopGamesResponse context)
        => Task.FromResult(context.Pagination.Cursor);
}
