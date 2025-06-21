using System.Runtime.CompilerServices;
using EHonda.Pagination.Core;
using JetBrains.Annotations;

namespace EHonda.Pagination.Sequential;

/// <summary>
/// Handles paginated resources by sequentially fetching pages. It uses a context object (<typeparamref name="TPaginationContext"/>)
/// to manage the state between page requests, allowing it to retrieve all items across multiple pages.
/// </summary>
/// <remarks>
/// The algorithm to retrieve all items is as follows:
/// <list type="number">
/// <item><description>Call <see cref="GetPageAsync"/> to retrieve the current page's context. The input context may be null for the first page.</description></item>
/// <item><description>Call <see cref="ExtractItemsAsync"/> with the retrieved context to get items from the current page.</description></item>
/// <item><description>Yield each item.</description></item>
/// <item><description>Call <see cref="NextPageExistsAsync"/> with the current context to determine if more pages are available. If so, repeat the process with the new context.</description></item>
/// </list>
/// </remarks>
/// <typeparam name="TPaginationContext">The type that holds the context for pagination. This context is typically updated after fetching each page and is used to fetch subsequent pages and determine if more pages exist.</typeparam>
/// <typeparam name="TItem">The type of the items to be retrieved.</typeparam>
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
