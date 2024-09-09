using Sequential.Composite;

namespace CursorBased;

public class NextPageChecker<TTransformedPage, TCursor> : INextPageChecker<PaginationContext<TTransformedPage, TCursor>>
{
    public Task<bool> NextPageExistsAsync(
        PaginationContext<TTransformedPage, TCursor> context,
        CancellationToken cancellationToken = default)
        => Task.FromResult(context.Cursor is not null);
}
