namespace EHonda.Pagination.Sequential.Composite.PageRetrievers;

/// <summary>
/// Retrieves a page of data using an asynchronous function.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
public class ByAsyncFunction<TPaginationContext> : IPageRetriever<TPaginationContext>
{
    private readonly Func<TPaginationContext?, CancellationToken, Task<TPaginationContext>> _pageRetriever;

    /// <summary>
    /// Initializes a new instance of the <see cref="ByAsyncFunction{TPaginationContext}"/> class.
    /// </summary>
    /// <param name="pageRetriever">The asynchronous function used to retrieve a page.</param>
    public ByAsyncFunction(Func<TPaginationContext?, CancellationToken, Task<TPaginationContext>> pageRetriever)
    {
        _pageRetriever = pageRetriever;
    }

    /// <inheritdoc />
    public Task<TPaginationContext> GetAsync(TPaginationContext? context, CancellationToken cancellationToken = default)
        => _pageRetriever(context, cancellationToken);
}
