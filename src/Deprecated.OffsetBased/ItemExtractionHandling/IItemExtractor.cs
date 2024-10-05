namespace Deprecated.OffsetBased.ItemExtractionHandling;

/// <summary>
/// Abstraction for extracting items from a transformed page (after pagination information extraction).
/// </summary>
/// <typeparam name="TTransformedPage">
///     The type of the transformed page after pagination information extraction.
/// </typeparam>
/// <typeparam name="TItem">The type of the items to extract.</typeparam>
[PublicAPI]
public interface IItemExtractor<in TTransformedPage, out TItem>
{
    /// <summary>
    /// Extracts all items from the given <paramref name="page"/>.
    /// </summary>
    /// <param name="page">The transformed page to extract the items from.</param>
    /// <param name="cancellationToken">Can be used to cancel the extraction.</param>
    /// <returns>An async enumerable of all items.</returns>
    IAsyncEnumerable<TItem> ExtractAsync(TTransformedPage page, CancellationToken cancellationToken = default);
}
