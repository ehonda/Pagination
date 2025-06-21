using Ardalis.GuardClauses;
using EHonda.Pagination.Sequential.Composite.ItemExtractors;
using EHonda.Pagination.Sequential.Composite.NextPageCheckers;
using EHonda.Pagination.Sequential.Composite.PageRetrievers;
using JetBrains.Annotations;

namespace EHonda.Pagination.Sequential.Composite;

/// <summary>
/// A builder for creating instances of <see cref="PaginationHandler{TPaginationContext, TItem}"/>.
/// This builder allows for fluent configuration of page retrieval, next page checking, and item extraction logic.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the context used for pagination. Must be a class.</typeparam>
/// <typeparam name="TItem">The type of items being paginated.</typeparam>
[PublicAPI]
public class PaginationHandlerBuilder<TPaginationContext, TItem>
    where TPaginationContext : class
{
    private IPageRetriever<TPaginationContext>? _pageRetriever;
    private INextPageChecker<TPaginationContext>? _nextPageChecker;
    private IItemExtractor<TPaginationContext, TItem>? _itemExtractor;

    /// <summary>
    /// Sets the page retriever component.
    /// </summary>
    /// <param name="pageRetriever">The page retriever to use.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TItem> WithPageRetriever(
        IPageRetriever<TPaginationContext> pageRetriever)
    {
        _pageRetriever = pageRetriever;
        return this;
    }

    /// <summary>
    /// Sets the page retriever using an asynchronous function.
    /// </summary>
    /// <param name="pageRetriever">The asynchronous function to retrieve a page.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TItem> WithPageRetriever(
        Func<TPaginationContext?, CancellationToken, Task<TPaginationContext>> pageRetriever)
        => WithPageRetriever(new PageRetrievers.ByAsyncFunction<TPaginationContext>(pageRetriever));

    /// <summary>
    /// Sets the page retriever using a synchronous function.
    /// </summary>
    /// <param name="pageRetriever">The synchronous function to retrieve a page.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TItem> WithPageRetriever(
        Func<TPaginationContext?, TPaginationContext> pageRetriever)
        => WithPageRetriever(new PageRetrievers.ByFunction<TPaginationContext>(pageRetriever));

    /// <summary>
    /// Sets the next page checker component.
    /// </summary>
    /// <param name="nextPageChecker">The next page checker to use.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TItem> WithNextPageChecker(
        INextPageChecker<TPaginationContext> nextPageChecker)
    {
        _nextPageChecker = nextPageChecker;
        return this;
    }

    /// <summary>
    /// Sets the next page checker using an asynchronous function.
    /// </summary>
    /// <param name="nextPageChecker">The asynchronous function to check for a next page.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TItem> WithNextPageChecker(
        Func<TPaginationContext, CancellationToken, Task<bool>> nextPageChecker)
        => WithNextPageChecker(new NextPageCheckers.ByAsyncFunction<TPaginationContext>(nextPageChecker));

    /// <summary>
    /// Sets the next page checker using a synchronous function.
    /// </summary>
    /// <param name="nextPageChecker">The synchronous function to check for a next page.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TItem> WithNextPageChecker(
        Func<TPaginationContext, bool> nextPageChecker)
        => WithNextPageChecker(new NextPageCheckers.ByFunction<TPaginationContext>(nextPageChecker));

    /// <summary>
    /// Sets the item extractor component.
    /// </summary>
    /// <param name="itemExtractor">The item extractor to use.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TItem> WithItemExtractor(
        IItemExtractor<TPaginationContext, TItem> itemExtractor)
    {
        _itemExtractor = itemExtractor;
        return this;
    }

    /// <summary>
    /// Sets the item extractor using an asynchronous function.
    /// </summary>
    /// <param name="itemExtractor">The asynchronous function to extract items.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TItem> WithItemExtractor(
        Func<TPaginationContext, CancellationToken, IAsyncEnumerable<TItem>> itemExtractor)
        => WithItemExtractor(new ItemExtractors.ByAsyncFunction<TPaginationContext, TItem>(itemExtractor));

    /// <summary>
    /// Sets the item extractor using a synchronous function.
    /// </summary>
    /// <param name="itemExtractor">The synchronous function to extract items.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TItem> WithItemExtractor(
        Func<TPaginationContext, IEnumerable<TItem>> itemExtractor)
        => WithItemExtractor(new ItemExtractors.ByFunction<TPaginationContext, TItem>(itemExtractor));

    /// <summary>
    /// Builds the <see cref="PaginationHandler{TPaginationContext, TItem}"/> instance.
    /// </summary>
    /// <returns>A new instance of <see cref="PaginationHandler{TPaginationContext, TItem}"/>.</returns>
    /// <exception cref="GuardClauseViolationException">Thrown if any of the required components (page retriever, next page checker, or item extractor) are not set.</exception>
    public PaginationHandler<TPaginationContext, TItem> Build()
    {
        Guard.Against.Null(_pageRetriever);
        Guard.Against.Null(_nextPageChecker);
        Guard.Against.Null(_itemExtractor);

        return new(_pageRetriever, _nextPageChecker, _itemExtractor);
    }
}
