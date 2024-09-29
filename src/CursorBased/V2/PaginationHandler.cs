namespace CursorBased.V2;

public abstract class PaginationHandler<TPaginationContext, TCursor, TItem>
    : Sequential.V2.PaginationHandler<TPaginationContext, TItem>
    where TPaginationContext : class, IPaginationContext<TCursor>
{
    private readonly NextPageChecker<TPaginationContext, TCursor> _nextPageChecker = new();
    
    protected override Task<bool> NextPageExistsAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default)
        => _nextPageChecker.NextPageExistsAsync(context, cancellationToken);
}
