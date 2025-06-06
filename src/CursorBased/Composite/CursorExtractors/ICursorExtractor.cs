namespace CursorBased.Composite.CursorExtractors;

/// <summary>
/// Defines a contract for extracting a cursor from a pagination context, defaulting to a string cursor type.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context. This type is contravariant.</typeparam>
public interface ICursorExtractor<in TPaginationContext> : ICursorExtractor<TPaginationContext, string>;

/// <summary>
/// Defines a contract for extracting a cursor from a pagination context.
/// </summary>
/// <typeparam name="TPaginationContext">The type of the pagination context. This type is contravariant.</typeparam>
/// <typeparam name="TCursor">The type of the cursor.</typeparam>
public interface ICursorExtractor<in TPaginationContext, TCursor>
{
    /// <summary>
    /// Asynchronously extracts a cursor from the given pagination context.
    /// </summary>
    /// <param name="context">The pagination context.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the extracted cursor, or <see langword="null"/> if no further cursor is available.</returns>
    Task<TCursor?> ExtractCursorAsync(TPaginationContext context);
}
