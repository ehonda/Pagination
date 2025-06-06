using System.Numerics;

namespace OffsetBased.Composite.OffsetStateExtractors;

// TODO: Do we want versions with just one type parameter for these as well? Probably not right, because they are not
//       designed to be inherited from (and the builder methods work with the two type param versions anyway).

// TODO: Should we make these sealed?

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
