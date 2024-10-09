using System.Runtime.CompilerServices;
using Core;
using JetBrains.Annotations;

namespace Sequential;

// TODO: Fix xml doc, it's not accurate anymore

/// <summary>
/// Retrieves all items from a paginated resource by sequentially going from one page to the next, as long as more pages
/// are available.
/// </summary>
/// <remarks>
/// The algorithm to retrieve all pages basically looks like this:
/// <list type="number">
/// <item>
///     Retrieve the next page.
/// </item>
/// <item>
///     Transform the page, typically to extract information about the next page, as well as the items on the page.
/// </item>
/// <item>
///     Yield all items on the page.
/// </item>
/// <item>
///     While there is a next page, repeat the previous three steps.
/// </item>
/// </list>
/// </remarks>
/// <typeparam name="TPage">The type of the transformed pages.</typeparam>
/// <typeparam name="TTransformedPage">The type of the transformed pages.</typeparam>
/// <typeparam name="TItem">The type of the items.</typeparam>
[PublicAPI]
public abstract class PaginationHandler<TPaginationContext, TItem> : IPaginationHandler<TItem>
    // TODO: It would be nicer to use an option type in the places where there might not be a context (instead of null
    //       like we do now), because we would not require this restriction.
    where TPaginationContext : class
{
    /// <inheritdoc />
    public async IAsyncEnumerable<TItem> GetAllItemsAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        TPaginationContext? context = null;

        while (context is null || await NextPageExistsAsync(context, cancellationToken))
        {
            context = await GetPageAsync(context, cancellationToken);

            await foreach (var item in ExtractItemsAsync(context, cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return item;
            }
        }
    }

    /// <summary>
    /// Retrieves the first page of the paginated resource.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>The first page.</returns>
    protected abstract Task<TPaginationContext> GetPageAsync(
        TPaginationContext? context,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks whether a next page exists.
    /// </summary>
    /// <param name="currentPage">The current page.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>Whether a next page exists.</returns>
    protected abstract Task<bool> NextPageExistsAsync(
        TPaginationContext context, CancellationToken cancellationToken = default);

    /// <summary>
    /// Extracts the items from the given <paramref name="page"/>.
    /// </summary>
    /// <param name="page">The page to extract the items from.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>An async enumerable of the items.</returns>
    protected abstract IAsyncEnumerable<TItem> ExtractItemsAsync(
        TPaginationContext context, CancellationToken cancellationToken = default);
}
