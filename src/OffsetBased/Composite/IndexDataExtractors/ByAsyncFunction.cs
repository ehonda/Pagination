using System.Numerics;

namespace OffsetBased.Composite.IndexDataExtractors;

public class ByAsyncFunction<TPaginationContext, TIndex> : IIndexDataExtractor<TPaginationContext, TIndex>
    where TPaginationContext : class
    where TIndex : INumber<TIndex>
{
    private readonly Func<TPaginationContext,CancellationToken,Task<IndexData<TIndex>>> _indexDataExtractor;

    public ByAsyncFunction(Func<TPaginationContext, CancellationToken, Task<IndexData<TIndex>>> indexDataExtractor)
    {
        _indexDataExtractor = indexDataExtractor;
    }
    
    public Task<IndexData<TIndex>> ExtractIndexDataAsync(TPaginationContext context,
        CancellationToken cancellationToken = default)
        => _indexDataExtractor(context, cancellationToken);
}