using System.Numerics;
using OffsetBased.Composite.IndexDataExtractors;
using Sequential.Composite.NextPageCheckers;

namespace OffsetBased.Composite;

public class NextPageChecker<TPaginationContext, TIndex> : INextPageChecker<TPaginationContext>
    where TPaginationContext : class
    where TIndex : INumber<TIndex>
{
    private readonly IIndexDataExtractor<TPaginationContext, TIndex> _indexDataExtractor;

    public NextPageChecker(IIndexDataExtractor<TPaginationContext, TIndex> indexDataExtractor)
    {
        _indexDataExtractor = indexDataExtractor;
    }

    public async Task<bool> NextPageExistsAsync(TPaginationContext context,
        CancellationToken cancellationToken = default)
    {
        var indexData = await _indexDataExtractor.ExtractIndexDataAsync(context, cancellationToken);

        return indexData.Offset < indexData.Total;
    }
}
