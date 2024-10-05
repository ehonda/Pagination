namespace Deprecated.OffsetBased.ItemExtractionHandling;

/// <inheritdoc />
/// <remarks>
/// Extraction is realized by the provided extraction function.
/// </remarks>
[PublicAPI]
public class ItemExtractorViaFunction<TTransformedPage, TItem> : ItemExtractorViaAsyncFunction<TTransformedPage, TItem>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ItemExtractorViaFunction{TTransformedPage, TItem}"/> class.
    /// </summary>
    /// <param name="extractItems">The function to extract items from a transformed page.</param>
    public ItemExtractorViaFunction(
        Func<TTransformedPage, CancellationToken, IEnumerable<TItem>> extractItems)
        : base((page, cancellationToken) => extractItems(page, cancellationToken).ToAsyncEnumerable())
    {
    }
}
