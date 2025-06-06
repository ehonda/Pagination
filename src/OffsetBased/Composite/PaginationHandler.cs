using System.Numerics;
using OffsetBased.Composite.OffsetStateExtractors;
using Sequential.Composite.ItemExtractors;
using Sequential.Composite.PageRetrievers;

namespace OffsetBased.Composite;

/// <summary>
/// A composite pagination handler for offset-based pagination.
/// This version defaults to an <see cref="int"/> for the index type.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
/// <typeparam name="TItem">The type of the items to be retrieved.</typeparam>
public class PaginationHandler<TPaginationContext, TItem>
    : PaginationHandler<TPaginationContext, int, TItem>
    where TPaginationContext : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationHandler{TPaginationContext, TItem}"/> class.
    /// </summary>
    /// <param name="pageRetriever">The strategy for retrieving pages.</param>
    /// <param name="offsetStateExtractor">The strategy for extracting offset state.</param>
    /// <param name="itemExtractor">The strategy for extracting items from a page.</param>
    public PaginationHandler(
        IPageRetriever<TPaginationContext> pageRetriever,
        IOffsetStateExtractor<TPaginationContext, int> offsetStateExtractor,
        IItemExtractor<TPaginationContext, TItem> itemExtractor)
        : base(pageRetriever, offsetStateExtractor, itemExtractor)
    {
    }
}

/// <summary>
/// A composite pagination handler for offset-based pagination.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
/// <typeparam name="TIndex">The type of the index used for pagination (e.g., int, long).</typeparam>
/// <typeparam name="TItem">The type of the items to be retrieved.</typeparam>
public class PaginationHandler<TPaginationContext, TIndex, TItem>
    : Sequential.Composite.PaginationHandler<TPaginationContext, TItem>
    where TPaginationContext : class
    where TIndex : INumber<TIndex>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationHandler{TPaginationContext, TIndex, TItem}"/> class.
    /// </summary>
    /// <param name="pageRetriever">The strategy for retrieving pages.</param>
    /// <param name="offsetStateExtractor">The strategy for extracting offset state.</param>
    /// <param name="itemExtractor">The strategy for extracting items from a page.</param>
    public PaginationHandler(
        IPageRetriever<TPaginationContext> pageRetriever,
        IOffsetStateExtractor<TPaginationContext, TIndex> offsetStateExtractor,
        IItemExtractor<TPaginationContext, TItem> itemExtractor)
        : base(pageRetriever, new NextPageChecker<TPaginationContext, TIndex>(offsetStateExtractor), itemExtractor)
    {
    }
}
