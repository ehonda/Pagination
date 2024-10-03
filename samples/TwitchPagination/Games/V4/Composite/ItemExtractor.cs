using Sequential.V2.Composite;
using Sequential.V2.Composite.ItemExtractors;

namespace TwitchPagination.Games.V4.Composite;

public class ItemExtractor : IItemExtractor<GetTopGamesResponse, Game>
{
    public IAsyncEnumerable<Game> ExtractItemsAsync(GetTopGamesResponse context,
        CancellationToken cancellationToken = default)
        => context.Data.ToAsyncEnumerable();
}
