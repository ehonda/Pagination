using Sequential.Composite.ItemExtractors;

namespace TwitchPagination.Games.Composite;

/// <summary>
/// <b>📖 EXAMPLE:</b> Implementation of an item extractor to use in the
/// <see cref="TopGamesPaginationHandler">composite pagination handler example</see>.
/// </summary>
public class ItemExtractor : IItemExtractor<GetTopGamesResponse, Game>
{
    public IAsyncEnumerable<Game> ExtractItemsAsync(GetTopGamesResponse context,
        CancellationToken cancellationToken = default)
        => context.Data.ToAsyncEnumerable();
}
