using Ardalis.GuardClauses;
using JetBrains.Annotations;
using Sequential.V2.Composite.ItemExtractors;
using Sequential.V2.Composite.NextPageCheckers;
using Sequential.V2.Composite.PageRetrievers;

namespace Sequential.V2.Composite;

[PublicAPI]
public class PaginationHandlerBuilder<TPaginationContext, TItem>
    where TPaginationContext : class
{
    private IPageRetriever<TPaginationContext>? _pageRetriever;
    private INextPageChecker<TPaginationContext>? _nextPageChecker;
    private IItemExtractor<TPaginationContext, TItem>? _itemExtractor;

    public PaginationHandlerBuilder<TPaginationContext, TItem> WithPageRetriever(
        IPageRetriever<TPaginationContext> pageRetriever)
    {
        _pageRetriever = pageRetriever;
        return this;
    }

    public PaginationHandlerBuilder<TPaginationContext, TItem> WithPageRetriever(
        Func<TPaginationContext?, CancellationToken, Task<TPaginationContext>> pageRetriever)
        => WithPageRetriever(new PageRetrievers.ByAsyncFunction<TPaginationContext>(pageRetriever));

    public PaginationHandlerBuilder<TPaginationContext, TItem> WithPageRetriever(
        Func<TPaginationContext?, TPaginationContext> pageRetriever)
        => WithPageRetriever(new PageRetrievers.ByFunction<TPaginationContext>(pageRetriever));

    public PaginationHandlerBuilder<TPaginationContext, TItem> WithNextPageChecker(
        INextPageChecker<TPaginationContext> nextPageChecker)
    {
        _nextPageChecker = nextPageChecker;
        return this;
    }

    public PaginationHandlerBuilder<TPaginationContext, TItem> WithNextPageChecker(
        Func<TPaginationContext, CancellationToken, Task<bool>> nextPageChecker)
        => WithNextPageChecker(new NextPageCheckers.ByAsyncFunction<TPaginationContext>(nextPageChecker));

    public PaginationHandlerBuilder<TPaginationContext, TItem> WithNextPageChecker(
        Func<TPaginationContext, bool> nextPageChecker)
        => WithNextPageChecker(new NextPageCheckers.ByFunction<TPaginationContext>(nextPageChecker));

    public PaginationHandlerBuilder<TPaginationContext, TItem> WithItemExtractor(
        IItemExtractor<TPaginationContext, TItem> itemExtractor)
    {
        _itemExtractor = itemExtractor;
        return this;
    }

    public PaginationHandlerBuilder<TPaginationContext, TItem> WithItemExtractor(
        Func<TPaginationContext, CancellationToken, IAsyncEnumerable<TItem>> itemExtractor)
        => WithItemExtractor(new ItemExtractors.ByAsyncFunction<TPaginationContext, TItem>(itemExtractor));

    public PaginationHandlerBuilder<TPaginationContext, TItem> WithItemExtractor(
        Func<TPaginationContext, IEnumerable<TItem>> itemExtractor)
        => WithItemExtractor(new ItemExtractors.ByFunction<TPaginationContext, TItem>(itemExtractor));

    public PaginationHandler<TPaginationContext, TItem> Build()
    {
        Guard.Against.Null(_pageRetriever);
        Guard.Against.Null(_nextPageChecker);
        Guard.Against.Null(_itemExtractor);

        return new(_pageRetriever, _nextPageChecker, _itemExtractor);
    }
}
