using System.Numerics;
using EHonda.Pagination.OffsetBased.Composite.OffsetStateExtractors;
using EHonda.Pagination.Sequential.Composite.NextPageCheckers;

namespace EHonda.Pagination.OffsetBased.Composite;

/// <summary>
/// Checks if a next page exists for offset-based pagination using an <see cref="IOffsetStateExtractor{TPaginationContext, TIndex}"/>.
/// This version defaults to an <see cref="int"/> for the index type.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
public class NextPageChecker<TPaginationContext>
    : NextPageChecker<TPaginationContext, int>
    where TPaginationContext : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NextPageChecker{TPaginationContext}"/> class.
    /// </summary>
    /// <param name="offsetStateExtractor">The extractor for offset state.</param>
    public NextPageChecker(IOffsetStateExtractor<TPaginationContext, int> offsetStateExtractor)
        : base(offsetStateExtractor)
    {
    }
}

/// <summary>
/// Checks if a next page exists for offset-based pagination using an <see cref="IOffsetStateExtractor{TPaginationContext, TIndex}"/>.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
/// <typeparam name="TIndex">The type of the index used for pagination (e.g., int, long).</typeparam>
public class NextPageChecker<TPaginationContext, TIndex> : INextPageChecker<TPaginationContext>
    where TPaginationContext : class
    where TIndex : INumber<TIndex>
{
    private readonly IOffsetStateExtractor<TPaginationContext, TIndex> _offsetStateExtractor;

    /// <summary>
    /// Initializes a new instance of the <see cref="NextPageChecker{TPaginationContext, TIndex}"/> class.
    /// </summary>
    /// <param name="offsetStateExtractor">The extractor for offset state.</param>
    public NextPageChecker(IOffsetStateExtractor<TPaginationContext, TIndex> offsetStateExtractor)
    {
        _offsetStateExtractor = offsetStateExtractor;
    }

    /// <inheritdoc />
    public async Task<bool> NextPageExistsAsync(TPaginationContext context,
        CancellationToken cancellationToken = default)
    {
        var state = await _offsetStateExtractor.ExtractOffsetStateAsync(context, cancellationToken);

        return state.Offset < state.Total;
    }
}
