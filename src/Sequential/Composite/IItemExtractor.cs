namespace Sequential.Composite;

public interface IItemExtractor<in TPage, out TItem>
{
    IAsyncEnumerable<TItem> ExtractItemsAsync(TPage page, CancellationToken cancellationToken = default);
}
