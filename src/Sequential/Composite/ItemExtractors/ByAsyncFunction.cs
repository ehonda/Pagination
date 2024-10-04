namespace Sequential.Composite.ItemExtractors;

public class ByAsyncFunction<TPaginationContext, TItem> : IItemExtractor<TPaginationContext, TItem>
{
    private readonly Func<TPaginationContext, CancellationToken, IAsyncEnumerable<TItem>> _itemExtractor;

    public ByAsyncFunction(Func<TPaginationContext, CancellationToken, IAsyncEnumerable<TItem>> itemExtractor)
    {
        _itemExtractor = itemExtractor;
    }

    public IAsyncEnumerable<TItem> ExtractItemsAsync(TPaginationContext context,
        CancellationToken cancellationToken = default)
        => _itemExtractor(context, cancellationToken);
}
