namespace Sequential.Composite.ItemExtractors;

/// <summary>
/// Extracts items from a pagination context using an asynchronous function.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
/// <typeparam name="TItem">The type of the item.</typeparam>
public class ByAsyncFunction<TPaginationContext, TItem> : IItemExtractor<TPaginationContext, TItem>
{
    private readonly Func<TPaginationContext, CancellationToken, IAsyncEnumerable<TItem>> _itemExtractor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ByAsyncFunction{TPaginationContext, TItem}"/> class.
    /// </summary>
    /// <param name="itemExtractor">The asynchronous function used to extract items.</param>
    public ByAsyncFunction(Func<TPaginationContext, CancellationToken, IAsyncEnumerable<TItem>> itemExtractor)
    {
        _itemExtractor = itemExtractor;
    }

    /// <inheritdoc />
    public IAsyncEnumerable<TItem> ExtractItemsAsync(TPaginationContext context,
        CancellationToken cancellationToken = default)
        => _itemExtractor(context, cancellationToken);
}
