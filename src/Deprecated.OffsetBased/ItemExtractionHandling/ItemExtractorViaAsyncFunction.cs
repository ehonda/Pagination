namespace Deprecated.OffsetBased.ItemExtractionHandling;

/// <inheritdoc />
/// <remarks>
/// Extraction is realized by the provided async extraction function.
/// </remarks>
[PublicAPI]
public class ItemExtractorViaAsyncFunction<TTransformedPage, TItem> : IItemExtractor<TTransformedPage, TItem>
{
    private readonly Func<TTransformedPage, CancellationToken, IAsyncEnumerable<TItem>> _extractItems;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemExtractorViaAsyncFunction{TTransformedPage, TItem}"/> class.
    /// </summary>
    /// <param name="extractItems">The function to extract items from a transformed page.</param>
    public ItemExtractorViaAsyncFunction(
        Func<TTransformedPage, CancellationToken, IAsyncEnumerable<TItem>> extractItems)
    {
        _extractItems = extractItems;
    }

    /// <inheritdoc />
    public IAsyncEnumerable<TItem> ExtractAsync(TTransformedPage page, CancellationToken cancellationToken = default)
        => _extractItems(page, cancellationToken);
}
