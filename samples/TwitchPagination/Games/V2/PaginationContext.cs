using CursorBased.V2;

namespace TwitchPagination.Games.V2;

public class PaginationContext : IPaginationContext<string>
{
    public PaginationContext(GetTopGamesResponse page)
    {
        Games = page.Data;
        Cursor = page.Pagination.Cursor;
    }
    
    public IReadOnlyList<Game> Games { get; }
    
    public string? Cursor { get; }
}
