namespace CursorBased.V4.Composite.CursorExtractors;

public class ByFunction<TPaginationContext, TCursor> : ByAsyncFunction<TPaginationContext, TCursor>
{
    public ByFunction(Func<TPaginationContext, TCursor?> cursorExtractor)
        : base((context, _) => Task.FromResult(cursorExtractor(context)))
    {
    }
}