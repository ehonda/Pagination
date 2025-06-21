using Ardalis.GuardClauses;
using EHonda.Pagination.CursorBased.Composite.CursorExtractors;
using EHonda.Pagination.Sequential.Composite.ItemExtractors;
using EHonda.Pagination.Sequential.Composite.PageRetrievers;

namespace EHonda.Pagination.CursorBased.Composite;

/// <summary>
/// A builder for creating instances of <see cref="PaginationHandler{TPaginationContext, TItem}"/>, 
/// defaulting to a <see cref="string"/> cursor type.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the context used for pagination. Must be a class.</typeparam>
/// <typeparam name="TItem">The type of items being paginated.</typeparam>
public class PaginationHandlerBuilder<TPaginationContext, TItem>
    : PaginationHandlerBuilder<TPaginationContext, string, TItem>
    where TPaginationContext : class;

/// <summary>
/// A builder for creating instances of <see cref="PaginationHandler{TPaginationContext, TCursor, TItem}"/>.
/// This builder allows for fluent configuration of page retrieval, cursor extraction, and item extraction logic.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the context used for pagination. Must be a class.</typeparam>
/// <typeparam name="TCursor">The type of the cursor used for pagination.</typeparam>
/// <typeparam name="TItem">The type of items being paginated.</typeparam>
public class PaginationHandlerBuilder<TPaginationContext, TCursor, TItem>
    where TPaginationContext : class
{
    private IPageRetriever<TPaginationContext>? _pageRetriever;
    private ICursorExtractor<TPaginationContext, TCursor>? _cursorExtractor;
    private IItemExtractor<TPaginationContext, TItem>? _itemExtractor;

    /// <summary>
    /// Sets the page retriever component.
    /// </summary>
    /// <param name="pageRetriever">The page retriever to use.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithPageRetriever(
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
    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithPageRetriever(
        Func<TPaginationContext?, CancellationToken, Task<TPaginationContext>> pageRetriever)
        => WithPageRetriever(new ByAsyncFunction<TPaginationContext>(pageRetriever));
    
    /// <summary>
    /// Sets the page retriever using a synchronous function.
    /// </summary>
    /// <param name="pageRetriever">The synchronous function to retrieve a page.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithPageRetriever(
        Func<TPaginationContext?, TPaginationContext> pageRetriever)
        => WithPageRetriever(new ByFunction<TPaginationContext>(pageRetriever));

    /// <summary>
    /// Sets the cursor extractor component.
    /// </summary>
    /// <param name="cursorExtractor">The cursor extractor to use.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithCursorExtractor(
        ICursorExtractor<TPaginationContext, TCursor> cursorExtractor)
    {
        _cursorExtractor = cursorExtractor;
        return this;
    }
    
    /// <summary>
    /// Sets the cursor extractor using an asynchronous function.
    /// </summary>
    /// <param name="cursorExtractor">The asynchronous function to extract the cursor.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithCursorExtractor(
        Func<TPaginationContext, CancellationToken, Task<TCursor?>> cursorExtractor)
        => WithCursorExtractor(new CursorExtractors.ByAsyncFunction<TPaginationContext, TCursor>(cursorExtractor));
    
    /// <summary>
    /// Sets the cursor extractor using a synchronous function.
    /// </summary>
    /// <param name="cursorExtractor">The synchronous function to extract the cursor.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithCursorExtractor(
        Func<TPaginationContext, TCursor?> cursorExtractor)
        => WithCursorExtractor(new CursorExtractors.ByFunction<TPaginationContext, TCursor>(cursorExtractor));

    /// <summary>
    /// Sets the item extractor component.
    /// </summary>
    /// <param name="itemExtractor">The item extractor to use.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithItemExtractor(
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
    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithItemExtractor(
        Func<TPaginationContext, CancellationToken, IAsyncEnumerable<TItem>> itemExtractor)
        => WithItemExtractor(new Sequential.Composite.ItemExtractors.ByAsyncFunction<TPaginationContext, TItem>(itemExtractor));
    
    /// <summary>
    /// Sets the item extractor using a synchronous function.
    /// </summary>
    /// <param name="itemExtractor">The synchronous function to extract items.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TCursor, TItem> WithItemExtractor(
        Func<TPaginationContext, IEnumerable<TItem>> itemExtractor)
        => WithItemExtractor(new Sequential.Composite.ItemExtractors.ByFunction<TPaginationContext, TItem>(itemExtractor));

    /// <summary>
    /// Builds the <see cref="PaginationHandler{TPaginationContext, TCursor, TItem}"/> instance.
    /// </summary>
    /// <returns>A new instance of <see cref="PaginationHandler{TPaginationContext, TCursor, TItem}"/>.</returns>
    /// <exception cref="GuardClauseViolationException">Thrown if any of the required components (page retriever, cursor extractor, or item extractor) are not set.</exception>
    public PaginationHandler<TPaginationContext, TCursor, TItem> Build()
    {
        Guard.Against.Null(_pageRetriever);
        Guard.Against.Null(_cursorExtractor);
        Guard.Against.Null(_itemExtractor);

        return new(_pageRetriever, _cursorExtractor, _itemExtractor);
    }
}
