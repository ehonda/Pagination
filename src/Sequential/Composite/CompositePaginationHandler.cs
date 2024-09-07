using JetBrains.Annotations;

namespace Sequential.Composite;

[PublicAPI]
public class CompositePaginationHandler<TPage, TTransformedPage, TItem>
    : PaginationHandler<TPage, TTransformedPage, TItem>
{
    private readonly IPageRetriever<TTransformedPage, TPage> _pageRetriever;
    private readonly IPaginationContextExtractor<TPage, TTransformedPage> _contextExtractor;
    private readonly IItemExtractor<TTransformedPage, TItem> _itemExtractor;

    public CompositePaginationHandler(
        IPageRetriever<TTransformedPage, TPage> pageRetriever,
        IPaginationContextExtractor<TPage, TTransformedPage> contextExtractor,
        IItemExtractor<TTransformedPage, TItem> itemExtractor)
    {
        _pageRetriever = pageRetriever;
        _contextExtractor = contextExtractor;
        _itemExtractor = itemExtractor;
    }
    
    protected override Task<TPage> GetPageAsync(
        TTransformedPage? currentPage,
        CancellationToken cancellationToken = default)
        => _pageRetriever.GetAsync(currentPage, cancellationToken);

    protected override Task<IPaginationContext<TTransformedPage>> ExtractContextAsync(
        TPage page,
        CancellationToken cancellationToken = default)
        => _contextExtractor.ExtractAsync(page, cancellationToken);
    
    protected override IAsyncEnumerable<TItem> ExtractItemsAsync(
        TTransformedPage page,
        CancellationToken cancellationToken = default)
        => _itemExtractor.ExtractItemsAsync(page, cancellationToken);
}
