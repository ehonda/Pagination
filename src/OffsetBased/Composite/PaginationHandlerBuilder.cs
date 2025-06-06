using System.Numerics;
using Ardalis.GuardClauses;
using OffsetBased.Composite.OffsetStateExtractors;
using Sequential.Composite.ItemExtractors;
using Sequential.Composite.PageRetrievers;

namespace OffsetBased.Composite;

/// <summary>
/// A builder for creating instances of <see cref="PaginationHandler{TPaginationContext, TItem}"/>.
/// This version defaults to an <see cref="int"/> for the index type.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
/// <typeparam name="TItem">The type of the items to be retrieved.</typeparam>
public class PaginationHandlerBuilder<TPaginationContext, TItem>
    : PaginationHandlerBuilder<TPaginationContext, int, TItem>
    where TPaginationContext : class;

/// <summary>
/// A builder for creating instances of <see cref="PaginationHandler{TPaginationContext, TIndex, TItem}"/>.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
/// <typeparam name="TIndex">The type of the index used for pagination (e.g., int, long).</typeparam>
/// <typeparam name="TItem">The type of the items to be retrieved.</typeparam>
public class PaginationHandlerBuilder<TPaginationContext, TIndex, TItem>
    where TPaginationContext : class
    where TIndex : INumber<TIndex>
{
    private IPageRetriever<TPaginationContext>? _pageRetriever;
    private IOffsetStateExtractor<TPaginationContext, TIndex>? _offsetStateExtractor;
    private IItemExtractor<TPaginationContext, TItem>? _itemExtractor;

    // It's a bit sad we have to repeat the stuff from Sequential.V2.Composite.PaginationHandlerBuilder here, but it
    // will not matter if we implement the source generation, so it's fine for now

    /// <summary>
    /// Sets the page retriever strategy.
    /// </summary>
    /// <param name="pageRetriever">The page retriever.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithPageRetriever(
        IPageRetriever<TPaginationContext> pageRetriever)
    {
        _pageRetriever = pageRetriever;
        return this;
    }

    /// <summary>
    /// Sets the page retriever strategy using an asynchronous function.
    /// </summary>
    /// <param name="pageRetriever">The asynchronous function to retrieve a page.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithPageRetriever(
        Func<TPaginationContext?, CancellationToken, Task<TPaginationContext>> pageRetriever)
        => WithPageRetriever(new ByAsyncFunction<TPaginationContext>(pageRetriever));

    /// <summary>
    /// Sets the page retriever strategy using a synchronous function.
    /// </summary>
    /// <param name="pageRetriever">The synchronous function to retrieve a page.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithPageRetriever(
        Func<TPaginationContext?, TPaginationContext> pageRetriever)
        => WithPageRetriever(new ByFunction<TPaginationContext>(pageRetriever));

    /// <summary>
    /// Sets the offset state extractor strategy.
    /// </summary>
    /// <param name="offsetStateExtractor">The offset state extractor.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithOffsetStateExtractor(
        IOffsetStateExtractor<TPaginationContext, TIndex> offsetStateExtractor)
    {
        _offsetStateExtractor = offsetStateExtractor;
        return this;
    }

    /// <summary>
    /// Sets the offset state extractor strategy using an asynchronous function.
    /// </summary>
    /// <param name="offsetStateExtractor">The asynchronous function to extract offset state.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithOffsetStateExtractor(
        Func<TPaginationContext, CancellationToken, Task<OffsetState<TIndex>>> offsetStateExtractor)
        => WithOffsetStateExtractor(
            new OffsetStateExtractors.ByAsyncFunction<TPaginationContext, TIndex>(offsetStateExtractor));

    /// <summary>
    /// Sets the offset state extractor strategy using a synchronous function.
    /// </summary>
    /// <param name="offsetStateExtractor">The synchronous function to extract offset state.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithOffsetStateExtractor(
        Func<TPaginationContext, OffsetState<TIndex>> offsetStateExtractor)
        => WithOffsetStateExtractor(new OffsetStateExtractors.ByFunction<TPaginationContext, TIndex>(offsetStateExtractor));

    /// <summary>
    /// Sets the item extractor strategy.
    /// </summary>
    /// <param name="itemExtractor">The item extractor.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithItemExtractor(
        IItemExtractor<TPaginationContext, TItem> itemExtractor)
    {
        _itemExtractor = itemExtractor;
        return this;
    }

    /// <summary>
    /// Sets the item extractor strategy using an asynchronous function.
    /// </summary>
    /// <param name="itemExtractor">The asynchronous function to extract items.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithItemExtractor(
        Func<TPaginationContext, CancellationToken, IAsyncEnumerable<TItem>> itemExtractor)
        => WithItemExtractor(
            new Sequential.Composite.ItemExtractors.ByAsyncFunction<TPaginationContext, TItem>(itemExtractor));

    /// <summary>
    /// Sets the item extractor strategy using a synchronous function.
    /// </summary>
    /// <param name="itemExtractor">The synchronous function to extract items.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public PaginationHandlerBuilder<TPaginationContext, TIndex, TItem> WithItemExtractor(
        Func<TPaginationContext, IEnumerable<TItem>> itemExtractor)
        => WithItemExtractor(
            new Sequential.Composite.ItemExtractors.ByFunction<TPaginationContext, TItem>(itemExtractor));

    /// <summary>
    /// Builds the <see cref="PaginationHandler{TPaginationContext, TIndex, TItem}"/> with the configured strategies.
    /// </summary>
    /// <returns>A new instance of <see cref="PaginationHandler{TPaginationContext, TIndex, TItem}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if any of the required strategies (page retriever, offset state extractor, or item extractor) are not set.</exception>
    public PaginationHandler<TPaginationContext, TIndex, TItem> Build()
    {
        Guard.Against.Null(_pageRetriever);
        Guard.Against.Null(_offsetStateExtractor);
        Guard.Against.Null(_itemExtractor);

        return new(_pageRetriever, _offsetStateExtractor, _itemExtractor);
    }
}
