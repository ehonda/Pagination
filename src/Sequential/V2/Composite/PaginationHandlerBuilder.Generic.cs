using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace Sequential.V2.Composite;

public class PaginationHandlerBuilder<TPaginationContext, TItem>
    : PaginationHandlerBuilder<
        PaginationHandlerBuilder<TPaginationContext, TItem>,
        TPaginationContext,
        TItem>
    where TPaginationContext : class;

[PublicAPI]
public class PaginationHandlerBuilder<TDerivedBuilder, TPaginationContext, TItem>
    where TDerivedBuilder : PaginationHandlerBuilder<TDerivedBuilder, TPaginationContext, TItem>
    where TPaginationContext : class
{
    private IPageRetriever<TPaginationContext>? _pageRetriever;
    private INextPageChecker<TPaginationContext>? _nextPageChecker;
    private IItemExtractor<TPaginationContext, TItem>? _itemExtractor;

    public TDerivedBuilder WithPageRetriever(IPageRetriever<TPaginationContext> pageRetriever)
    {
        _pageRetriever = pageRetriever;
        return (TDerivedBuilder)this;
    }

    public TDerivedBuilder WithNextPageChecker(INextPageChecker<TPaginationContext> nextPageChecker)
    {
        _nextPageChecker = nextPageChecker;
        return (TDerivedBuilder)this;
    }

    public TDerivedBuilder WithItemExtractor(IItemExtractor<TPaginationContext, TItem> itemExtractor)
    {
        _itemExtractor = itemExtractor;
        return (TDerivedBuilder)this;
    }

    public PaginationHandler<TPaginationContext, TItem> Build()
    {
        Guard.Against.Null(_pageRetriever);
        Guard.Against.Null(_nextPageChecker);
        Guard.Against.Null(_itemExtractor);
        
        return new(_pageRetriever, _nextPageChecker, _itemExtractor);
    }
}
