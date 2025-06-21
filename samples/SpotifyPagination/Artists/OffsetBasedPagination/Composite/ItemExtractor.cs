using EHonda.Pagination.Sequential.Composite.ItemExtractors;

namespace SpotifyPagination.Artists.OffsetBasedPagination.Composite;

/// <summary>
/// <b>📖 EXAMPLE:</b> Implementation of an item extractor to use in the
/// <see cref="TopGamesPaginationHandler">composite pagination handler example</see>.
/// </summary>
public class ItemExtractor : IItemExtractor<GetAlbumsResponse, Album>
{
    public IAsyncEnumerable<Album> ExtractItemsAsync(GetAlbumsResponse context,
        CancellationToken cancellationToken = default)
        => context.Items.ToAsyncEnumerable();
}
