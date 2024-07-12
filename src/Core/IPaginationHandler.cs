using JetBrains.Annotations;

namespace Core;

/// <summary>
/// A pagination handler is responsible for retrieving all items from a paginated resource.
/// </summary>
/// <typeparam name="TItem">The type of the items.</typeparam>
[PublicAPI]
public interface IPaginationHandler<out TItem>
{
    /// <summary>
    /// Retrieves all items from a paginated resource, using the given <paramref name="httpClient"/>.
    /// </summary>
    /// <param name="httpClient">The http client to use for the requests.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>An async enumerable of all items.</returns>
    IAsyncEnumerable<TItem> GetAllItemsAsync(HttpClient httpClient, CancellationToken cancellationToken = default);
}
