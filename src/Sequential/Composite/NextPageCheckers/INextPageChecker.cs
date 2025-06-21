using JetBrains.Annotations;

namespace EHonda.Pagination.Sequential.Composite.NextPageCheckers;

/// <summary>
/// Defines a contract for checking if a next page exists in a pagination context.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context. This type is contravariant.</typeparam>
[PublicAPI]
public interface INextPageChecker<in TPaginationContext>
{
    /// <summary>
    /// Asynchronously checks if a next page exists.
    /// </summary>
    /// <param name="context">The pagination context.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <c>true</c> if a next page exists; otherwise, <c>false</c>.</returns>
    Task<bool> NextPageExistsAsync(TPaginationContext context, CancellationToken cancellationToken = default);
}
