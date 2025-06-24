using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestUtilities.Games;

public class GamesClient
{
    private readonly IReadOnlyList<string> _items;

    public int TotalCount => _items.Count;

    public GamesClient(IReadOnlyList<string> items)
    {
        _items = items;
    }

    public Task<GetTopGamesResponse> GetGamesByCursor(string? cursor, int pageSize)
    {
        var startIndex = 0;
        if (cursor != null && int.TryParse(cursor, out var cursorIndex))
        {
            startIndex = cursorIndex;
        }

        var pageItems = _items.Skip(startIndex).Take(pageSize).ToList();

        string? nextCursor = null;
        if (startIndex + pageSize < _items.Count)
        {
            nextCursor = (startIndex + pageSize).ToString();
        }

        var response = new GetTopGamesResponse(pageItems, nextCursor);
        return Task.FromResult(response);
    }

    public Task<IReadOnlyList<string>> GetGamesByOffset(int offset, int limit)
    {
        var pageItems = _items.Skip(offset).Take(limit).ToList();
        return Task.FromResult<IReadOnlyList<string>>(pageItems);
    }
}
