using EHonda.Pagination.CursorBased.Composite.CursorExtractors;
using EHonda.Pagination.Sequential.Composite.NextPageCheckers;

namespace EHonda.Pagination.CursorBased.Composite;

/// <summary>
/// Checks for the existence of a next page in a cursor-based pagination context, using a string cursor.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
public class NextPageChecker<TPaginationContext>
    : NextPageChecker<TPaginationContext, string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NextPageChecker{TPaginationContext}"/> class.
    /// </summary>
    /// <param name="cursorExtractor">The cursor extractor to determine if a next page exists.</param>
    public NextPageChecker(ICursorExtractor<TPaginationContext> cursorExtractor)
        : base(cursorExtractor)
    {
    }
}

/// <summary>
/// Checks for the existence of a next page in a cursor-based pagination context.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context.</typeparam>
/// <typeparam name="TCursor">The type of the cursor.</typeparam>
public class NextPageChecker<TPaginationContext, TCursor>
    : INextPageChecker<TPaginationContext>
{
    private readonly ICursorExtractor<TPaginationContext, TCursor> _cursorExtractor;

    /// <summary>
    /// Initializes a new instance of the <see cref="NextPageChecker{TPaginationContext, TCursor}"/> class.
    /// </summary>
    /// <param name="cursorExtractor">The cursor extractor to determine if a next page exists (which is the case iff. the extracted cursor is not <see langword="null"/>).</param>
    public NextPageChecker(ICursorExtractor<TPaginationContext, TCursor> cursorExtractor)
    {
        _cursorExtractor = cursorExtractor;
    }

    /// <inheritdoc />
    public async Task<bool> NextPageExistsAsync(TPaginationContext context,
        CancellationToken cancellationToken = default)
        => await _cursorExtractor.ExtractCursorAsync(context) is not null;
}
