using System.Numerics;
using OffsetBased.Composite.IndexDataExtractors;
using Sequential.Composite;
using Sequential.Composite.ItemExtractors;
using Sequential.Composite.PageRetrievers;

namespace OffsetBased.Composite;

public class PaginationHandler<TPaginationContext, TIndex, TItem> 
    : PaginationHandler<TPaginationContext, TItem>
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
