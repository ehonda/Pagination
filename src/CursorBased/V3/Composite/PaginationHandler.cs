using JetBrains.Annotations;
using Sequential.V2.Composite;
using Sequential.V2.Composite.ItemExtractors;
using Sequential.V2.Composite.PageRetrievers;

namespace CursorBased.V3.Composite;

// TODO: How can we reuse the Sequential.V2.Composite.PaginationHandler here? Or do we just not use it?
[PublicAPI]
public class PaginationHandler<TPaginationContext, TCursor, TItem>
    : V3.PaginationHandler<TPaginationContext, TCursor, TItem>
    where TPaginationContext : class
{
    private readonly IPageRetriever<TPaginationContext> _pageRetriever;
    private readonly IItemExtractor<TPaginationContext, TItem> _itemExtractor;

    public PaginationHandler(
        IPageRetriever<TPaginationContext> pageRetriever,
        ICursorExtractor<TPaginationContext, TCursor> cursorExtractor,
        IItemExtractor<TPaginationContext, TItem> itemExtractor)
        : base(cursorExtractor)
    {
        _pageRetriever = pageRetriever;
        _itemExtractor = itemExtractor;
    }

    protected override Task<TPaginationContext> GetPageAsync(TPaginationContext? context,
        CancellationToken cancellationToken = default)
        => _pageRetriever.GetAsync(context, cancellationToken);

    protected override IAsyncEnumerable<TItem> ExtractItemsAsync(TPaginationContext context,
        CancellationToken cancellationToken = default)
        => _itemExtractor.ExtractItemsAsync(context, cancellationToken);
}
