using System.Numerics;

namespace OffsetBased.Composite.IndexDataExtractors;

public interface IIndexDataExtractor<in TPaginationContext>
    : IIndexDataExtractor<TPaginationContext, int>
    where TPaginationContext : class;

public interface IIndexDataExtractor<in TPaginationContext, TIndex>
    where TPaginationContext : class
    where TIndex : INumber<TIndex>
{
    Task<IndexData<TIndex>> ExtractIndexDataAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default);
}
