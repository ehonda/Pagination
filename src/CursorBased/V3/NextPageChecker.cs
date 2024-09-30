using Sequential.Composite;

namespace CursorBased.V3;

public class NextPageChecker<TPaginationContext, TCursor> : INextPageChecker<TPaginationContext>
{
    private readonly ICursorExtractor<TPaginationContext, TCursor> _cursorExtractor;

    public NextPageChecker(ICursorExtractor<TPaginationContext, TCursor> cursorExtractor)
    {
        _cursorExtractor = cursorExtractor;
    }

    public async Task<bool> NextPageExistsAsync(TPaginationContext context,
        CancellationToken cancellationToken = default)
        => await _cursorExtractor.ExtractCursorAsync(context) is not null;
}
