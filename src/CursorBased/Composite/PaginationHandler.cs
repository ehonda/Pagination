using CursorBased.Composite.CursorExtractors;
using JetBrains.Annotations;
using Sequential.Composite.ItemExtractors;
using Sequential.Composite.PageRetrievers;

namespace CursorBased.Composite;

public class PaginationHandler<TPaginationContext, TItem>
    : PaginationHandler<TPaginationContext, string, TItem>
    where TPaginationContext : class
{
    public PaginationHandler(IPageRetriever<TPaginationContext> pageRetriever,
        ICursorExtractor<TPaginationContext, string> cursorExtractor,
        IItemExtractor<TPaginationContext, TItem> itemExtractor) : base(pageRetriever, cursorExtractor, itemExtractor)
    {
    }
}

// TODO: We probably want to use namespace prefixes here for readability (even though it's not necessary)
[PublicAPI]
public class PaginationHandler<TPaginationContext, TCursor, TItem>
    : Sequential.Composite.PaginationHandler<TPaginationContext, TItem>
    where TPaginationContext : class
{
    public PaginationHandler(
        IPageRetriever<TPaginationContext> pageRetriever,
        ICursorExtractor<TPaginationContext, TCursor> cursorExtractor,
        IItemExtractor<TPaginationContext, TItem> itemExtractor)
        : base(pageRetriever, new NextPageChecker<TPaginationContext, TCursor>(cursorExtractor), itemExtractor)
    {
    }
}
