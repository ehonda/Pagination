using Sequential.Composite.ItemExtractors;

namespace TwitchPagination.Games.Composite;

public class ItemExtractor : IItemExtractor<GetTopGamesResponse, Game>
{
    public IAsyncEnumerable<Game> ExtractItemsAsync(GetTopGamesResponse context,
        CancellationToken cancellationToken = default)
        => context.Data.ToAsyncEnumerable();
}
