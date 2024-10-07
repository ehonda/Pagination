using System.Numerics;

namespace OffsetBased.Composite.IndexDataExtractors;

public interface IIndexDataExtractor<TPaginationContext, TIndex>
    where TPaginationContext : class
    where TIndex : INumber<TIndex>
{
    Task<IndexData<TIndex>> ExtractIndexDataAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default);
}
