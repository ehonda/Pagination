using System.Numerics;

namespace OffsetBased;

/// <summary>
/// Extends the sequential pagination handler to support offset-based pagination.
/// It uses an index (<typeparamref name="TIndex"/>) to track the current position (offset) and the total number of items.
/// </summary>
/// <typeparam name="TPaginationContext">The type that holds the context for pagination. This context is typically updated after fetching each page and is used to fetch subsequent pages and determine if more pages exist.</typeparam>
/// <typeparam name="TItem">The type of the items to be retrieved.</typeparam>
public abstract class PaginationHandler<TPaginationContext, TItem>
    : PaginationHandler<TPaginationContext, int, TItem>
    where TPaginationContext : class;

/// <summary>
/// Extends the sequential pagination handler to support offset-based pagination.
/// It uses an index (<typeparamref name="TIndex"/>) to track the current position (offset) and the total number of items.
/// </summary>
/// <typeparam name="TPaginationContext">The type that holds the context for pagination. This context is typically updated after fetching each page and is used to fetch subsequent pages and determine if more pages exist.</typeparam>
/// <typeparam name="TIndex">The type of the index used for pagination (e.g., int, long).</typeparam>
/// <typeparam name="TItem">The type of the items to be retrieved.</typeparam>
public abstract class PaginationHandler<TPaginationContext, TIndex, TItem>
    : Sequential.PaginationHandler<TPaginationContext, TItem>
    where TPaginationContext : class
    where TIndex : INumber<TIndex>
{
    /// <inheritdoc />
    protected override async Task<bool> NextPageExistsAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default)
    {
        var state = await ExtractOffsetStateAsync(context, cancellationToken);

        return state.Offset < state.Total;
    }

    /// <summary>
    /// Extracts the offset state (offset and total) from the given pagination context.
    /// </summary>
    /// <param name="context">The current pagination context.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>The extracted offset state.</returns>
    protected abstract Task<OffsetState<TIndex>> ExtractOffsetStateAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default);
}
