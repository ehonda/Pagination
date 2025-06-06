namespace Sequential.Composite.PageRetrievers;

// TODO: Is there any benefit in implementing this via the async variant? Are there any downsides? Maybe benchmark the
//       difference
/// <summary>
/// Retrieves a page of data using a synchronous function.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
public class ByFunction<TPaginationContext> : ByAsyncFunction<TPaginationContext>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ByFunction{TPaginationContext}"/> class.
    /// </summary>
    /// <param name="pageRetriever">The synchronous function used to retrieve a page.</param>
    public ByFunction(Func<TPaginationContext?, TPaginationContext> pageRetriever)
        : base((context, _) => Task.FromResult(pageRetriever(context)))
    {
    }
}
