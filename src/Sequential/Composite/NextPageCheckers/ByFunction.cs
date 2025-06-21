namespace EHonda.Pagination.Sequential.Composite.NextPageCheckers;

/// <summary>
/// Checks for the existence of a next page using a synchronous function.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
public class ByFunction<TPaginationContext> : INextPageChecker<TPaginationContext>
{
    private readonly Func<TPaginationContext, bool> _nextPageChecker;

    /// <summary>
    /// Initializes a new instance of the <see cref="ByFunction{TPaginationContext}"/> class.
    /// </summary>
    /// <param name="nextPageChecker">The synchronous function used to check for a next page.</param>
    public ByFunction(Func<TPaginationContext, bool> nextPageChecker)
    {
        _nextPageChecker = nextPageChecker;
    }

    /// <inheritdoc />
    public Task<bool> NextPageExistsAsync(TPaginationContext context, CancellationToken cancellationToken = default)
        => Task.FromResult(_nextPageChecker(context));
}
