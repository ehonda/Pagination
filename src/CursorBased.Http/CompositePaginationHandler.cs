using CursorBased.Composite;
using Sequential.Composite;

namespace CursorBased.Http;

public abstract class CompositePaginationHandler<TTransformedPage, TCursor, TItem>
    : CompositePaginationHandler<HttpResponseMessage, TTransformedPage, TCursor, TItem>
{
    protected CompositePaginationHandler(
        PageRetriever<TTransformedPage, TCursor> pageRetriever,
        IPaginationContextExtractor<HttpResponseMessage, PaginationContext<TTransformedPage, TCursor>> contextExtractor,
        IItemExtractor<PaginationContext<TTransformedPage, TCursor>, TItem> itemExtractor)
        : base(pageRetriever, contextExtractor, itemExtractor)
    {
    }
}
