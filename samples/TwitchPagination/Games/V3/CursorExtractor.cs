using CursorBased.V3;

namespace TwitchPagination.Games.V3;

public class CursorExtractor : ICursorExtractor<GetTopGamesResponse, string>
{
    public Task<string?> ExtractCursorAsync(GetTopGamesResponse context)
        => Task.FromResult(context.Pagination.Cursor);
}
