using Sequential.Composite;

namespace CursorBased.Composite;

public class CompositePaginationHandler<TPage, TTransformedPage, TCursor, TItem>
    : CompositePaginationHandler<TPage, PaginationContext<TTransformedPage, TCursor>, TItem>
{
    public CompositePaginationHandler(
        IPageRetriever<PaginationContext<TTransformedPage, TCursor>, TPage> pageRetriever,
        IPaginationContextExtractor<TPage, PaginationContext<TTransformedPage, TCursor>> contextExtractor,
        IItemExtractor<PaginationContext<TTransformedPage, TCursor>, TItem> itemExtractor)
        : base(pageRetriever, contextExtractor, new NextPageChecker<TTransformedPage, TCursor>(), itemExtractor)
    {
    }
}
