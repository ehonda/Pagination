namespace EHonda.Pagination.CursorBased.Composite.CursorExtractors;

/// <summary>
/// Extracts a cursor from a pagination context using an asynchronous function.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
/// <typeparam name="TCursor">The type of the cursor.</typeparam>
public class ByAsyncFunction<TPaginationContext, TCursor> : ICursorExtractor<TPaginationContext, TCursor>
{
    private readonly Func<TPaginationContext, CancellationToken, Task<TCursor?>> _cursorExtractor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ByAsyncFunction{TPaginationContext, TCursor}"/> class.
    /// </summary>
    /// <param name="cursorExtractor">The asynchronous function used to extract the cursor.</param>
    public ByAsyncFunction(Func<TPaginationContext, CancellationToken, Task<TCursor?>> cursorExtractor)
    {
        _cursorExtractor = cursorExtractor;
    }

    /// <inheritdoc />
    public Task<TCursor?> ExtractCursorAsync(TPaginationContext context)
        => _cursorExtractor(context, CancellationToken.None); // Consider passing a CancellationToken if appropriate
}
