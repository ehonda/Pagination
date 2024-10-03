using Ardalis.GuardClauses;
using CursorBased.V4.Composite.CursorExtractors;
using Sequential.V2.Composite.ItemExtractors;
using Sequential.V2.Composite.PageRetrievers;

namespace CursorBased.V4.Composite;

public class PaginationHandlerBuilder<TPaginationContext, TCursor, TItem>
    where TPaginationContext : class
{
    private IPageRetriever<TPaginationContext>? _pageRetriever;
    private ICursorExtractor<TPaginationContext, TCursor>? _cursorExtractor;
    private IItemExtractor<TPaginationContext, TItem>? _itemExtractor;

    // It's a bit sad we have to repeat the stuff from Sequential.V2.Composite.PaginationHandlerBuilder here, but it
    // will not matter if we implement the source generation, so it's fine for now
    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithPageRetriever(
        IPageRetriever<TPaginationContext> pageRetriever)
    {
        _pageRetriever = pageRetriever;
        return this;
    }
    
    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithPageRetriever(
        Func<TPaginationContext?, CancellationToken, Task<TPaginationContext>> pageRetriever)
        => WithPageRetriever(new ByAsyncFunction<TPaginationContext>(pageRetriever));
    
    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithPageRetriever(
        Func<TPaginationContext?, TPaginationContext> pageRetriever)
        => WithPageRetriever(new ByFunction<TPaginationContext>(pageRetriever));

    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithCursorExtractor(
        ICursorExtractor<TPaginationContext, TCursor> cursorExtractor)
    {
        _cursorExtractor = cursorExtractor;
        return this;
    }
    
    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithCursorExtractor(
        Func<TPaginationContext, CancellationToken, Task<TCursor?>> cursorExtractor)
        => WithCursorExtractor(new CursorExtractors.ByAsyncFunction<TPaginationContext, TCursor>(cursorExtractor));
    
    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithCursorExtractor(
        Func<TPaginationContext, TCursor?> cursorExtractor)
        => WithCursorExtractor(new CursorExtractors.ByFunction<TPaginationContext, TCursor>(cursorExtractor));

    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithItemExtractor(
        IItemExtractor<TPaginationContext, TItem> itemExtractor)
    {
        _itemExtractor = itemExtractor;
        return this;
    }
    
    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithItemExtractor(
        Func<TPaginationContext, CancellationToken, IAsyncEnumerable<TItem>> itemExtractor)
        => WithItemExtractor(new Sequential.V2.Composite.ItemExtractors
            .ByAsyncFunction<TPaginationContext, TItem>(itemExtractor));
    
    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithItemExtractor(
        Func<TPaginationContext, IEnumerable<TItem>> itemExtractor)
        => WithItemExtractor(new Sequential.V2.Composite.ItemExtractors
            .ByFunction<TPaginationContext, TItem>(itemExtractor));

    public PaginationHandler<TPaginationContext, TCursor, TItem> Build()
    {
        Guard.Against.Null(_pageRetriever);
        Guard.Against.Null(_cursorExtractor);
        Guard.Against.Null(_itemExtractor);

        return new(_pageRetriever, _cursorExtractor, _itemExtractor);
    }
}
