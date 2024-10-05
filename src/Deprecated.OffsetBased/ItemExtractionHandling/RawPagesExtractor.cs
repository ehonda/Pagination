using System.Runtime.CompilerServices;

namespace Deprecated.OffsetBased.ItemExtractionHandling;

/// <inheritdoc />
/// <remarks>
/// Extraction is realized by directly returning the page as the sole extracted item.
/// </remarks>
[PublicAPI]
public class RawPagesExtractor<TItem> : IItemExtractor<TItem, TItem>
{
    /// <inheritdoc />
    public async IAsyncEnumerable<TItem> ExtractAsync(
        TItem page,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        yield return page;
    }
}
