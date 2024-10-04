using JetBrains.Annotations;

namespace Sequential.Composite.ItemExtractors;

[PublicAPI]
public interface IItemExtractor<in TPaginationContext, out TItem>
{
    IAsyncEnumerable<TItem> ExtractItemsAsync(TPaginationContext context,
        CancellationToken cancellationToken = default);
}
