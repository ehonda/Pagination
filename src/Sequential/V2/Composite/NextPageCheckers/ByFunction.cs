namespace Sequential.V2.Composite.NextPageCheckers;

public class ByFunction<TPaginationContext> : INextPageChecker<TPaginationContext>
{
    private readonly Func<TPaginationContext, bool> _nextPageChecker;

    public ByFunction(Func<TPaginationContext, bool> nextPageChecker)
    {
        _nextPageChecker = nextPageChecker;
    }

    public Task<bool> NextPageExistsAsync(TPaginationContext context, CancellationToken cancellationToken = default)
        => Task.FromResult(_nextPageChecker(context));
}