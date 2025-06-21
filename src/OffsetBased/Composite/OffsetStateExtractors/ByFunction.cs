using System.Numerics;

namespace EHonda.Pagination.OffsetBased.Composite.OffsetStateExtractors;

/// <summary>
/// Extracts offset-based pagination state using a synchronous function.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
/// <typeparam name="TIndex">The type of the index used for pagination (e.g., int, long).</typeparam>
public class ByFunction<TPaginationContext, TIndex> : ByAsyncFunction<TPaginationContext, TIndex>
    where TPaginationContext : class
    where TIndex : INumber<TIndex>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ByFunction{TPaginationContext, TIndex}"/> class.
    /// </summary>
    /// <param name="offsetStateExtractor">The synchronous function to extract the offset state.</param>
    public ByFunction(Func<TPaginationContext, OffsetState<TIndex>> offsetStateExtractor)
        : base((context, _) => Task.FromResult(offsetStateExtractor(context)))
    {
    }
}
