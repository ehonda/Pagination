using CursorBased.Composite.CursorExtractors;
using Sequential.Composite.NextPageCheckers;

namespace CursorBased.Composite;

public class NextPageChecker<TPaginationContext>
    : NextPageChecker<TPaginationContext, string>
{
    public NextPageChecker(ICursorExtractor<TPaginationContext> cursorExtractor)
        : base(cursorExtractor)
    {
    }
}

public class NextPageChecker<TPaginationContext, TCursor>
    : INextPageChecker<TPaginationContext>
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
