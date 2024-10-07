using System.Numerics;
using OffsetBased.Composite.IndexDataExtractors;
using Sequential.Composite.ItemExtractors;
using Sequential.Composite.PageRetrievers;

namespace OffsetBased.Composite;

public class PaginationHandler<TPaginationContext, TItem>
    : PaginationHandler<TPaginationContext, int, TItem>
    where TPaginationContext : class
{
    public PaginationHandler(
        IPageRetriever<TPaginationContext> pageRetriever,
        IIndexDataExtractor<TPaginationContext, int> indexDataExtractor,
        IItemExtractor<TPaginationContext, TItem> itemExtractor)
        : base(pageRetriever, indexDataExtractor, itemExtractor)
    {
    }
}

public class PaginationHandler<TPaginationContext, TIndex, TItem>
    : Sequential.Composite.PaginationHandler<TPaginationContext, TItem>
    where TPaginationContext : class
    where TIndex : INumber<TIndex>
{
    public PaginationHandler(
        IPageRetriever<TPaginationContext> pageRetriever,
        IIndexDataExtractor<TPaginationContext, TIndex> indexDataExtractor,
        IItemExtractor<TPaginationContext, TItem> itemExtractor)
        : base(pageRetriever, new NextPageChecker<TPaginationContext, TIndex>(indexDataExtractor), itemExtractor)
    {
    }
}
