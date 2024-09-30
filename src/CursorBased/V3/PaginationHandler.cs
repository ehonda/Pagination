namespace CursorBased.V3;

public abstract class PaginationHandler<TPaginationContext, TCursor, TItem>
    : Sequential.V2.PaginationHandler<TPaginationContext, TItem>
    where TPaginationContext : class
{
    private readonly NextPageChecker<TPaginationContext, TCursor> _nextPageChecker;

    protected PaginationHandler(ICursorExtractor<TPaginationContext, TCursor> cursorExtractor)
    {
        _nextPageChecker = new(cursorExtractor);
    }

    protected override Task<bool> NextPageExistsAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default)
        => _nextPageChecker.NextPageExistsAsync(context, cancellationToken);
}
