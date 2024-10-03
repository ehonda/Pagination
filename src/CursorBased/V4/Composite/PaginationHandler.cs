using CursorBased.V4.Composite.CursorExtractors;
using JetBrains.Annotations;
using Sequential.V2.Composite;
using Sequential.V2.Composite.ItemExtractors;
using Sequential.V2.Composite.PageRetrievers;

namespace CursorBased.V4.Composite;

// TODO: We probably want to use namespace prefixes here for readability (even though it's not necessary)
[PublicAPI]
public class PaginationHandler<TPaginationContext, TCursor, TItem>
    : PaginationHandler<TPaginationContext, TItem>
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
