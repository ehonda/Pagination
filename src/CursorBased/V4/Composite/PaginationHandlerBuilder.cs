using Ardalis.GuardClauses;
using Sequential.V2.Composite;

namespace CursorBased.V4.Composite;

public class PaginationHandlerBuilder<TPaginationContext, TCursor, TItem>
    where TPaginationContext : class
{
    private IPageRetriever<TPaginationContext>? _pageRetriever;
    private ICursorExtractor<TPaginationContext, TCursor>? _cursorExtractor;
    private IItemExtractor<TPaginationContext, TItem>? _itemExtractor;

    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithPageRetriever(
        IPageRetriever<TPaginationContext> pageRetriever)
    {
        _pageRetriever = pageRetriever;
        return this;
    }

    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithCursorExtractor(
        ICursorExtractor<TPaginationContext, TCursor> cursorExtractor)
    {
        _cursorExtractor = cursorExtractor;
        return this;
    }

    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithItemExtractor(
        IItemExtractor<TPaginationContext, TItem> itemExtractor)
    {
        _itemExtractor = itemExtractor;
        return this;
    }

    public PaginationHandler<TPaginationContext, TCursor, TItem> Build()
    {
        Guard.Against.Null(_pageRetriever);
        Guard.Against.Null(_cursorExtractor);
        Guard.Against.Null(_itemExtractor);

        return new(_pageRetriever, _cursorExtractor, _itemExtractor);
    }
}
