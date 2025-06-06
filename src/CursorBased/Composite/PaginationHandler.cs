using CursorBased.Composite.CursorExtractors;
using JetBrains.Annotations;
using Sequential.Composite.ItemExtractors;
using Sequential.Composite.PageRetrievers;

namespace CursorBased.Composite;

/// <summary>
/// Handles cursor-based pagination by composing page retrieval, cursor extraction, and item extraction logic.
/// This variant defaults to using a <see cref="string"/> for the cursor type.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the context used for pagination. Must be a class.</typeparam>
/// <typeparam name="TItem">The type of items being paginated.</typeparam>
public class PaginationHandler<TPaginationContext, TItem>
    : PaginationHandler<TPaginationContext, string, TItem>
    where TPaginationContext : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationHandler{TPaginationContext, TItem}"/> class.
    /// </summary>
    /// <param name="pageRetriever">The component responsible for retrieving pages of data.</param>
    /// <param name="cursorExtractor">The component responsible for extracting the cursor for the next page.</param>
    /// <param name="itemExtractor">The component responsible for extracting items from a page context.</param>
    public PaginationHandler(IPageRetriever<TPaginationContext> pageRetriever,
        ICursorExtractor<TPaginationContext, string> cursorExtractor,
        IItemExtractor<TPaginationContext, TItem> itemExtractor) : base(pageRetriever, cursorExtractor, itemExtractor)
    {
    }
}

/// <summary>
/// Handles cursor-based pagination by composing page retrieval, cursor extraction, and item extraction logic.
/// This class extends the <see cref="Sequential.Composite.PaginationHandler{TPaginationContext, TItem}"/>,
/// using a <see cref="NextPageChecker{TPaginationContext, TCursor}"/> that relies on the provided <paramref name="cursorExtractor"/>.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the context used for pagination. Must be a class.</typeparam>
/// <typeparam name="TCursor">The type of the cursor used for pagination.</typeparam>
/// <typeparam name="TItem">The type of items being paginated.</typeparam>
[PublicAPI]
public class PaginationHandler<TPaginationContext, TCursor, TItem>
    : Sequential.Composite.PaginationHandler<TPaginationContext, TItem>
    where TPaginationContext : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationHandler{TPaginationContext, TCursor, TItem}"/> class.
    /// </summary>
    /// <param name="pageRetriever">The component responsible for retrieving pages of data.</param>
    /// <param name="cursorExtractor">The component responsible for extracting the cursor for the next page.</param>
    /// <param name="itemExtractor">The component responsible for extracting items from a page context.</param>
    public PaginationHandler(
        IPageRetriever<TPaginationContext> pageRetriever,
        ICursorExtractor<TPaginationContext, TCursor> cursorExtractor,
        IItemExtractor<TPaginationContext, TItem> itemExtractor)
        : base(pageRetriever, new NextPageChecker<TPaginationContext, TCursor>(cursorExtractor), itemExtractor)
    {
    }
}
