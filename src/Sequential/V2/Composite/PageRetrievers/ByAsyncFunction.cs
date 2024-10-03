namespace Sequential.V2.Composite.PageRetrievers;

public class ByAsyncFunction<TPaginationContext> : IPageRetriever<TPaginationContext>
{
    private readonly Func<TPaginationContext?, CancellationToken, Task<TPaginationContext>> _pageRetriever;
    
    public ByAsyncFunction(Func<TPaginationContext?, CancellationToken, Task<TPaginationContext>> pageRetriever)
    {
        _pageRetriever = pageRetriever;
    }
    
    public Task<TPaginationContext> GetAsync(TPaginationContext? context, CancellationToken cancellationToken = default)
        => _pageRetriever(context, cancellationToken);
}
