namespace Sequential.V2.Composite.NextPageCheckers;

public class ByAsyncFunction<TPaginationContext> : INextPageChecker<TPaginationContext>
{
    private readonly Func<TPaginationContext, CancellationToken, Task<bool>> _nextPageChecker;

    public ByAsyncFunction(Func<TPaginationContext, CancellationToken, Task<bool>> nextPageChecker)
    {
        _nextPageChecker = nextPageChecker;
    }

    public Task<bool> NextPageExistsAsync(TPaginationContext context, CancellationToken cancellationToken = default)
        => _nextPageChecker(context, cancellationToken);
}