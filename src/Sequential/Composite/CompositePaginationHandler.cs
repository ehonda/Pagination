using JetBrains.Annotations;

namespace Sequential.Composite;

[PublicAPI]
public class CompositePaginationHandler<TPage, TPaginationContext, TItem>
    : PaginationHandler<TPage, TPaginationContext, TItem>
    where TPaginationContext : class
{
    private readonly IPageRetriever<TPaginationContext, TPage> _pageRetriever;
    private readonly IPaginationContextExtractor<TPage, TPaginationContext> _contextExtractor;
    private readonly INextPageChecker<TPaginationContext> _nextPageChecker;
    private readonly IItemExtractor<TPaginationContext, TItem> _itemExtractor;

    public CompositePaginationHandler(
        IPageRetriever<TPaginationContext, TPage> pageRetriever,
        IPaginationContextExtractor<TPage, TPaginationContext> contextExtractor,
        INextPageChecker<TPaginationContext> nextPageChecker,
        IItemExtractor<TPaginationContext, TItem> itemExtractor)
    {
        _pageRetriever = pageRetriever;
        _contextExtractor = contextExtractor;
        _nextPageChecker = nextPageChecker;
        _itemExtractor = itemExtractor;
    }
    
    protected override Task<TPage> GetPageAsync(
        TPaginationContext? context,
        CancellationToken cancellationToken = default)
        => _pageRetriever.GetAsync(context, cancellationToken);

    protected override Task<TPaginationContext> ExtractContextAsync(
        TPage page,
        CancellationToken cancellationToken = default)
        => _contextExtractor.ExtractAsync(page, cancellationToken);
    
    protected override Task<bool> NextPageExistsAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default)
        => _nextPageChecker.NextPageExistsAsync(context, cancellationToken);
    
    protected override IAsyncEnumerable<TItem> ExtractItemsAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default)
        => _itemExtractor.ExtractItemsAsync(context, cancellationToken);
}
