using JetBrains.Annotations;

namespace Sequential.Composite;

[PublicAPI]
public interface IItemExtractor<in TPaginationContext, out TItem>
{
    // TODO: Consistent naming. ExtractItemsAsync, or ExtractAsync? - ItemExtractor or ItemsExtractor?
    IAsyncEnumerable<TItem> ExtractItemsAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default);
}
