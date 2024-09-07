namespace OffsetBased.Fluent;

/// <summary>
/// A fluent API for constructing a <see cref="OffsetBasedOriginalPaginationHandler{TTransformedPage,TItem}"/>.
/// </summary>
[PublicAPI]
public static class OffsetBasedPaginationHandlerBuilder
{
    /// <summary>
    /// Creates a new instance of <see cref="OffsetBasedPaginationHandlerBuilder{TTransformedPage, TItem}"/>.
    /// </summary>
    /// <typeparam name="TTransformedPage">
    ///     The type of the transformed page after pagination information extraction.
    /// </typeparam>
    /// <typeparam name="TItem">The type of the items on the page.</typeparam>
    /// <returns>A new instance of <see cref="OffsetBasedPaginationHandlerBuilder{TTransformedPage, TItem}"/>.</returns>
    public static OffsetBasedPaginationHandlerBuilder<TTransformedPage, TItem> Create<TTransformedPage, TItem>()
        => new();
}
