using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Core;

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
/// <typeparam name="TTransformedPage">The type of the transformed pages.</typeparam>
/// <typeparam name="TItem">The type of the items.</typeparam>
[PublicAPI]
public abstract class SequentialPaginationHandler<TTransformedPage, TItem> : IPaginationHandler<TItem>
{
    /// <inheritdoc />
    public async IAsyncEnumerable<TItem> GetAllItemsAsync(
        HttpClient httpClient, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var currentPage = await GetFirstPageAsync(httpClient, cancellationToken);
        var transformedCurrentPage = await TransformPageAsync(currentPage, cancellationToken);

        await foreach (var item in ExtractItemsAsync(transformedCurrentPage, cancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return item;
        }

        while (await NextPageExistsAsync(httpClient, transformedCurrentPage, cancellationToken))
        {
            currentPage = await GetNextPageAsync(httpClient, transformedCurrentPage, cancellationToken);
            transformedCurrentPage = await TransformPageAsync(currentPage, cancellationToken);

            await foreach (var item in ExtractItemsAsync(transformedCurrentPage, cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return item;
            }
        }
    }

    /// <summary>
    /// Retrieves the first page of the paginated resource.
    /// </summary>
    /// <param name="httpClient">The http client to use for the request.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>The first page.</returns>
    protected abstract Task<HttpResponseMessage> GetFirstPageAsync(
        HttpClient httpClient, CancellationToken cancellationToken = default);

    /// <summary>
    /// Transforms the given <paramref name="page"/>, typically to extract information about the next page, as well as
    /// the items on the page.
    /// </summary>
    /// <param name="page">The page to transform.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>The transformed page.</returns>
    protected abstract Task<TTransformedPage> TransformPageAsync(
        HttpResponseMessage page, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a next page exists.
    /// </summary>
    /// <param name="httpClient">The http client to use, in case a request needs to be made.</param>
    /// <param name="currentPage">The current page.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>Whether a next page exists.</returns>
    protected abstract Task<bool> NextPageExistsAsync(
        HttpClient httpClient, TTransformedPage currentPage, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the next page.
    /// </summary>
    /// <param name="httpClient">The http client to use for the request.</param>
    /// <param name="currentPage">The current page.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>The next page.</returns>
    protected abstract Task<HttpResponseMessage> GetNextPageAsync(
        HttpClient httpClient, TTransformedPage currentPage, CancellationToken cancellationToken = default);

    /// <summary>
    /// Extracts the items from the given <paramref name="page"/>.
    /// </summary>
    /// <param name="page">The page to extract the items from.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>An async enumerable of the items.</returns>
    protected abstract IAsyncEnumerable<TItem> ExtractItemsAsync(
        TTransformedPage page, CancellationToken cancellationToken = default);
}
