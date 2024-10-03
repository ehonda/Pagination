namespace Sequential.V2.Composite.PageRetrievers;

// TODO: Is there any benefit in implementing this via the async variant? Are there any downsides? Maybe benchmark the
//       difference
public class ByFunction<TPaginationContext> : ByAsyncFunction<TPaginationContext>
{
    public ByFunction(Func<TPaginationContext?, TPaginationContext> pageRetriever)
        : base((context, _) => Task.FromResult(pageRetriever(context)))
    {
    }
}
