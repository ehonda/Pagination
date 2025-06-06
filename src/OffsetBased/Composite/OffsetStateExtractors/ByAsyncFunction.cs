using System.Numerics;

namespace OffsetBased.Composite.OffsetStateExtractors;

/// <summary>
/// Extracts offset-based pagination state using an asynchronous function.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
/// <typeparam name="TIndex">The type of the index used for pagination (e.g., int, long).</typeparam>
public class ByAsyncFunction<TPaginationContext, TIndex> : IOffsetStateExtractor<TPaginationContext, TIndex>
    where TPaginationContext : class
    where TIndex : INumber<TIndex>
{
    private readonly Func<TPaginationContext, CancellationToken, Task<OffsetState<TIndex>>> _offsetStateExtractor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ByAsyncFunction{TPaginationContext, TIndex}"/> class.
    /// </summary>
    /// <param name="offsetStateExtractor">The asynchronous function to extract the offset state.</param>
    public ByAsyncFunction(Func<TPaginationContext, CancellationToken, Task<OffsetState<TIndex>>> offsetStateExtractor)
    {
        _offsetStateExtractor = offsetStateExtractor;
    }
    
    /// <inheritdoc />
    public Task<OffsetState<TIndex>> ExtractOffsetStateAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default)
        => _offsetStateExtractor(context, cancellationToken);
}
