using System.Numerics;

namespace EHonda.Pagination.OffsetBased.Composite.OffsetStateExtractors;

/// <summary>
/// Defines a contract for extracting offset-based pagination state.
/// This version defaults to an <see cref="int"/> for the index type.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
public interface IOffsetStateExtractor<in TPaginationContext>
    : IOffsetStateExtractor<TPaginationContext, int>
    where TPaginationContext : class;

/// <summary>
/// Defines a contract for extracting offset-based pagination state.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
/// <typeparam name="TIndex">The type of the index used for pagination (e.g., int, long).</typeparam>
public interface IOffsetStateExtractor<in TPaginationContext, TIndex>
    where TPaginationContext : class
    where TIndex : INumber<TIndex>
{
    /// <summary>
    /// Asynchronously extracts the offset state from the given pagination context.
    /// </summary>
    /// <param name="context">The pagination context.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="OffsetState{TIndex}"/>.</returns>
    Task<OffsetState<TIndex>> ExtractOffsetStateAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default);
}
