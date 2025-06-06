using JetBrains.Annotations;

namespace Sequential.Composite.ItemExtractors;

/// <summary>
/// Defines a contract for extracting items from a pagination context.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context. This type is contravariant.</typeparam>
/// <typeparam name="TItem">The type of the item. This type is covariant.</typeparam>
[PublicAPI]
public interface IItemExtractor<in TPaginationContext, out TItem>
{
    /// <summary>
    /// Asynchronously extracts items from the given pagination context.
    /// </summary>
    /// <param name="context">The pagination context.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An asynchronous enumerable of items.</returns>
    IAsyncEnumerable<TItem> ExtractItemsAsync(TPaginationContext context,
        CancellationToken cancellationToken = default);
}
