using Sequential.Composite;

namespace ArrayPaginationHandler;

public class NextPageChecker : INextPageChecker<IEnumerator<int>>
{
    public Task<bool> NextPageExistsAsync(
        IEnumerator<int> context,
        CancellationToken cancellationToken = default)
        => Task.FromResult(context.MoveNext());
}
