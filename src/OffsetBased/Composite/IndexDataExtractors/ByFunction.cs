using System.Numerics;

namespace OffsetBased.Composite.IndexDataExtractors;

// TODO: Do we want versions with just one type parameter for these as well? Probably not right, because they are not
//       designed to be inherited from (and the builder methods work with the two type param versions anyway).

// TODO: Should we make these sealed?

public class ByFunction<TPaginationContext, TIndex> : ByAsyncFunction<TPaginationContext, TIndex>
    where TPaginationContext : class
    where TIndex : INumber<TIndex>
{
    public ByFunction(Func<TPaginationContext, IndexData<TIndex>> indexDataExtractor)
        : base((context, _) => Task.FromResult(indexDataExtractor(context)))
    {
    }
}
