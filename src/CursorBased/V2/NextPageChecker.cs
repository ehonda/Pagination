using Sequential.Composite;

namespace CursorBased.V2;

public class NextPageChecker<TPaginationContext, TCursor> : INextPageChecker<TPaginationContext>
    where TPaginationContext : IPaginationContext<TCursor>
{
    public Task<bool> NextPageExistsAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default)
        => Task.FromResult(context.Cursor is not null);
}
