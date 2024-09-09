namespace CursorBased;

public abstract class PaginationHandler<TPage, TTransformedPage, TCursor, TItem>
    : Sequential.PaginationHandler<TPage, PaginationContext<TTransformedPage, TCursor>, TItem>
{
    private readonly NextPageChecker<TTransformedPage, TCursor> _nextPageChecker = new();

    protected override Task<bool> NextPageExistsAsync(
        PaginationContext<TTransformedPage, TCursor> context,
        CancellationToken cancellationToken = default)
        => _nextPageChecker.NextPageExistsAsync(context, cancellationToken);
}
