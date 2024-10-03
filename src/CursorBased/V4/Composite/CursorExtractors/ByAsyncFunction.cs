namespace CursorBased.V4.Composite.CursorExtractors;

public class ByAsyncFunction<TPaginationContext, TCursor> : ICursorExtractor<TPaginationContext, TCursor>
{
    private readonly Func<TPaginationContext, CancellationToken, Task<TCursor?>> _cursorExtractor;

    public ByAsyncFunction(Func<TPaginationContext, CancellationToken, Task<TCursor?>> cursorExtractor)
    {
        _cursorExtractor = cursorExtractor;
    }

    public Task<TCursor?> ExtractCursorAsync(TPaginationContext context)
        => _cursorExtractor(context, CancellationToken.None);
}