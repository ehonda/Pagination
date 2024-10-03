namespace Sequential.V2.Composite.ItemExtractors;

public class ByFunction<TPaginationContext, TItem> : IItemExtractor<TPaginationContext, TItem>
{
    private readonly Func<TPaginationContext, IEnumerable<TItem>> _itemExtractor;

    public ByFunction(Func<TPaginationContext, IEnumerable<TItem>> itemExtractor)
    {
        _itemExtractor = itemExtractor;
    }

    public IAsyncEnumerable<TItem> ExtractItemsAsync(TPaginationContext context,
        CancellationToken cancellationToken = default)
        => _itemExtractor(context).ToAsyncEnumerable();
}
