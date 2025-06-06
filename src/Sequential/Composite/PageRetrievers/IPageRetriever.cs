using JetBrains.Annotations;

namespace Sequential.Composite.PageRetrievers;

/// <summary>
/// Defines a contract for retrieving a page of data in a pagination context.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
[PublicAPI]
public interface IPageRetriever<TPaginationContext>
{
    /// <summary>
    /// Asynchronously retrieves a page of data.
    /// </summary>
    /// <param name="context">The current pagination context, or <see langword="null"/> if retrieving the first page.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the pagination context for the retrieved page.</returns>
    Task<TPaginationContext> GetAsync(TPaginationContext? context, CancellationToken cancellationToken = default);
}
