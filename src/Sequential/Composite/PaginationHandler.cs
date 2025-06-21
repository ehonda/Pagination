using EHonda.Pagination.Sequential.Composite.PageRetrievers;
using JetBrains.Annotations;

namespace EHonda.Pagination.Sequential.Composite;

/// <summary>
/// Handles sequential pagination by composing page retrieval, next page checking, and item extraction logic.
/// This class extends the base <see cref="Sequential.PaginationHandler{TPaginationContext,TItem}"/>,
/// providing a concrete implementation of its abstract methods using the provided components.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the context used for pagination. Must be a class.</typeparam>
/// <typeparam name="TItem">The type of items being paginated.</typeparam>
[PublicAPI]
public class PaginationHandler<TPaginationContext, TItem>
    : Sequential.PaginationHandler<TPaginationContext, TItem>
    where TPaginationContext : class
{
    private readonly IPageRetriever<TPaginationContext> _pageRetriever;
    private readonly NextPageCheckers.INextPageChecker<TPaginationContext> _nextPageChecker;
    private readonly ItemExtractors.IItemExtractor<TPaginationContext, TItem> _itemExtractor;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationHandler{TPaginationContext, TItem}"/> class.
    /// </summary>
    /// <param name="pageRetriever">The component responsible for retrieving pages of data.</param>
    /// <param name="nextPageChecker">The component responsible for checking if a next page exists.</param>
    /// <param name="itemExtractor">The component responsible for extracting items from a page context.</param>
    public PaginationHandler(
        IPageRetriever<TPaginationContext> pageRetriever,
        NextPageCheckers.INextPageChecker<TPaginationContext> nextPageChecker,
        ItemExtractors.IItemExtractor<TPaginationContext, TItem> itemExtractor)
    {
        _pageRetriever = pageRetriever;
        _nextPageChecker = nextPageChecker;
        _itemExtractor = itemExtractor;
    }

    /// <inheritdoc />
    protected override Task<TPaginationContext> GetPageAsync(
        TPaginationContext? context,
        CancellationToken cancellationToken = default)
        => _pageRetriever.GetAsync(context, cancellationToken);

    /// <inheritdoc />
    protected override Task<bool> NextPageExistsAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default)
        => _nextPageChecker.NextPageExistsAsync(context, cancellationToken);

    /// <inheritdoc />
    protected override IAsyncEnumerable<TItem> ExtractItemsAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default)
        => _itemExtractor.ExtractItemsAsync(context, cancellationToken);
}
