using System.Numerics;

namespace OffsetBased.Composite.IndexDataExtractors;

public class ByFunction<TPaginationContext, TIndex> : ByAsyncFunction<TPaginationContext, TIndex>
    where TPaginationContext : class
    where TIndex : INumber<TIndex>
{
    public ByFunction(Func<TPaginationContext, IndexData<TIndex>> indexDataExtractor)
        : base((context, _) => Task.FromResult(indexDataExtractor(context)))
    {
    }
}