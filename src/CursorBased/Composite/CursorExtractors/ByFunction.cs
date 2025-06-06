namespace CursorBased.Composite.CursorExtractors;

/// <summary>
/// Extracts a cursor from a pagination context using a synchronous function.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
/// <typeparam name="TCursor">The type of the cursor.</typeparam>
public class ByFunction<TPaginationContext, TCursor> : ByAsyncFunction<TPaginationContext, TCursor>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ByFunction{TPaginationContext, TCursor}"/> class.
    /// </summary>
    /// <param name="cursorExtractor">The synchronous function used to extract the cursor.</param>
    public ByFunction(Func<TPaginationContext, TCursor?> cursorExtractor)
        : base((context, _) => Task.FromResult(cursorExtractor(context)))
    {
    }
}
