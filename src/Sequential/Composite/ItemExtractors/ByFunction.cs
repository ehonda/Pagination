namespace EHonda.Pagination.Sequential.Composite.ItemExtractors;

/// <summary>
/// Extracts items from a pagination context using a synchronous function.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
/// <typeparam name="TItem">The type of the item.</typeparam>
public class ByFunction<TPaginationContext, TItem> : IItemExtractor<TPaginationContext, TItem>
{
    private readonly Func<TPaginationContext, IEnumerable<TItem>> _itemExtractor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ByFunction{TPaginationContext, TItem}"/> class.
    /// </summary>
    /// <param name="itemExtractor">The synchronous function used to extract items.</param>
    public ByFunction(Func<TPaginationContext, IEnumerable<TItem>> itemExtractor)
    {
        _itemExtractor = itemExtractor;
    }

    /// <inheritdoc />
    public IAsyncEnumerable<TItem> ExtractItemsAsync(TPaginationContext context,
        CancellationToken cancellationToken = default)
        => _itemExtractor(context).ToAsyncEnumerable();
}
