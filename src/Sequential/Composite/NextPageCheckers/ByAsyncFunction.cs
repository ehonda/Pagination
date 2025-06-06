namespace Sequential.Composite.NextPageCheckers;

/// <summary>
/// Checks for the existence of a next page using an asynchronous function.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
public class ByAsyncFunction<TPaginationContext> : INextPageChecker<TPaginationContext>
{
    private readonly Func<TPaginationContext, CancellationToken, Task<bool>> _nextPageChecker;

    /// <summary>
    /// Initializes a new instance of the <see cref="ByAsyncFunction{TPaginationContext}"/> class.
    /// </summary>
    /// <param name="nextPageChecker">The asynchronous function used to check for a next page.</param>
    public ByAsyncFunction(Func<TPaginationContext, CancellationToken, Task<bool>> nextPageChecker)
    {
        _nextPageChecker = nextPageChecker;
    }

    /// <inheritdoc />
    public Task<bool> NextPageExistsAsync(TPaginationContext context, CancellationToken cancellationToken = default)
        => _nextPageChecker(context, cancellationToken);
}
