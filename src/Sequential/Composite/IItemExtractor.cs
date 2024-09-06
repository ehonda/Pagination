namespace Sequential.Composite;

public interface IItemExtractor<in TPage, out TItem>
{
    // TODO: Consistent naming. ExtractItemsAsync, or ExtractAsync? - ItemExtractor or ItemsExtractor?
    IAsyncEnumerable<TItem> ExtractItemsAsync(TPage page, CancellationToken cancellationToken = default);
}
