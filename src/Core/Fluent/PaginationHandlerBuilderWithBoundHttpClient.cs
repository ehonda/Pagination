using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace Core.Fluent;

/// <summary>
/// Base class for builders for <see cref="IPaginationHandler{TItem}"/> instances.
/// </summary>
/// <remarks>
/// The builder can be used to either build a <see cref="IPaginationHandler{TItem}"/> instance or to directly build and
/// retrieve all items from the paginated source via <see cref="GetAllItemsAsync"/>, if the <see cref="HttpClient"/> has
/// been set via <see cref="WithHttpClient"/>.
/// </remarks>
/// <typeparam name="TItem">The type of the items to be extracted from the paginated source.</typeparam>
[PublicAPI]
// TODO: Fix that awful long name
public abstract class PaginationHandlerBuilderWithBoundHttpClient<TDerivedBuilder, TItem>
    : PaginationHandlerBuilder<TItem>
    where TDerivedBuilder : PaginationHandlerBuilderWithBoundHttpClient<TDerivedBuilder, TItem>
{
    private const string HTTP_CLIENT_NOT_SET_MESSAGE
        = $"The {nameof(HttpClient)} must be set via {nameof(WithHttpClient)} when using {nameof(GetAllItemsAsync)} "
          + $"with a {nameof(PaginationHandlerBuilder<TItem>)}.";

    private HttpClient? HttpClient { get; set; }

    /// <summary>
    /// Sets the <see cref="HttpClient"/> to be used for retrieving items from the paginated source via
    /// <see cref="GetAllItemsAsync"/>.
    /// </summary>
    /// <param name="httpClient">
    ///     The <see cref="HttpClient"/> to be used for retrieving items from the paginated source.
    /// </param>
    /// <returns>The modified builder instance.</returns>
    public TDerivedBuilder WithHttpClient(HttpClient httpClient)
    {
        HttpClient = httpClient;
        return (TDerivedBuilder)this;
    }

    // TODO: Can we extract this to something like a "bound pagination handler"? It seems weird to mix this with the builder
    /// <summary>
    /// Retrieves all items from the paginated source.
    /// </summary>
    /// <remarks>
    /// The <see cref="HttpClient"/> must be set via <see cref="WithHttpClient"/> before calling this method. Items are
    /// then retrieved from the paginated source using the <see cref="IPaginationHandler{TItem}"/> instance built by
    /// <see cref="Build"/> and subsequently calling <see cref="IPaginationHandler{TItem}.GetAllItemsAsync"/>.
    /// </remarks>
    /// <param name="cancellationToken">Used to cancel the retrieval of items from the paginated source.</param>
    /// <returns>An asynchronous enumerable of all items from the paginated source.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <see cref="HttpClient"/> has not been set via <see cref="WithHttpClient"/>.
    /// </exception>
    public IAsyncEnumerable<TItem> GetAllItemsAsync(CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(
            HttpClient,
            message: HTTP_CLIENT_NOT_SET_MESSAGE);

        return Build().GetAllItemsAsync(HttpClient, cancellationToken);
    }
}
