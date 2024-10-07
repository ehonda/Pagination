using System.Numerics;
using Ardalis.GuardClauses;
using OffsetBased.Composite.IndexDataExtractors;
using Sequential.Composite.ItemExtractors;
using Sequential.Composite.PageRetrievers;

namespace OffsetBased.Composite;

public class PaginationHandlerBuilder<TPaginationContext, TIndex, TItem>
    where TPaginationContext : class
    where TIndex : INumber<TIndex>
{
    private IPageRetriever<TPaginationContext>? _pageRetriever;
    private IIndexDataExtractor<TPaginationContext, TIndex>? _indexDataExtractor;
    private IItemExtractor<TPaginationContext, TItem>? _itemExtractor;

    // It's a bit sad we have to repeat the stuff from Sequential.V2.Composite.PaginationHandlerBuilder here, but it
    // will not matter if we implement the source generation, so it's fine for now
    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithPageRetriever(
        IPageRetriever<TPaginationContext> pageRetriever)
    {
        _pageRetriever = pageRetriever;
        return this;
    }

    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithPageRetriever(
        Func<TPaginationContext?, CancellationToken, Task<TPaginationContext>> pageRetriever)
        => WithPageRetriever(new ByAsyncFunction<TPaginationContext>(pageRetriever));

    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithPageRetriever(
        Func<TPaginationContext?, TPaginationContext> pageRetriever)
        => WithPageRetriever(new ByFunction<TPaginationContext>(pageRetriever));

    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithIndexDataExtractor(
        IIndexDataExtractor<TPaginationContext, TIndex> indexDataExtractor)
    {
        _indexDataExtractor = indexDataExtractor;
        return this;
    }

    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithIndexDataExtractor(
        Func<TPaginationContext, CancellationToken, Task<IndexData<TIndex>>> indexDataExtractor)
        => WithIndexDataExtractor(
            new IndexDataExtractors.ByAsyncFunction<TPaginationContext, TIndex>(indexDataExtractor));

    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithIndexDataExtractor(
        Func<TPaginationContext, IndexData<TIndex>> indexDataExtractor)
        => WithIndexDataExtractor(new IndexDataExtractors.ByFunction<TPaginationContext, TIndex>(indexDataExtractor));

    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithItemExtractor(
        IItemExtractor<TPaginationContext, TItem> itemExtractor)
    {
        _itemExtractor = itemExtractor;
        return this;
    }

    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithItemExtractor(
        Func<TPaginationContext, CancellationToken, IAsyncEnumerable<TItem>> itemExtractor)
        => WithItemExtractor(
            new Sequential.Composite.ItemExtractors.ByAsyncFunction<TPaginationContext, TItem>(itemExtractor));

    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithItemExtractor(
        Func<TPaginationContext, IEnumerable<TItem>> itemExtractor)
        => WithItemExtractor(
            new Sequential.Composite.ItemExtractors.ByFunction<TPaginationContext, TItem>(itemExtractor));

    public PaginationHandler<TPaginationContext, TIndex, TItem> Build()
    {
        Guard.Against.Null(_pageRetriever);
        Guard.Against.Null(_indexDataExtractor);
        Guard.Against.Null(_itemExtractor);

        return new(_pageRetriever, _indexDataExtractor, _itemExtractor);
    }
}
