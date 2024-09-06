using Core.Fluent;

namespace OffsetBased.Fluent;

/// <summary>
/// A fluent API to bind a <see cref="OffsetBasedPaginationHandler{TTransformedPage, TItem}"/> to a <see cref="HttpClient"/>.
/// </summary>
[PublicAPI]
public static class PaginationHandlerSetupExtensions
{
    /// <summary>
    /// Binds a <see cref="OffsetBasedPaginationHandlerBuilder{TTransformedPage, TItem}"/> to a
    /// <see cref="HttpClient"/>.
    /// </summary>
    /// <param name="fluentBuilderSetup">
    ///     The setup containing the <see cref="HttpClient"/> to bind the pagination handler to.
    /// </param>
    /// <typeparam name="TTransformedPage">
    ///     The type of the transformed page after pagination information extraction.
    /// </typeparam>
    /// <typeparam name="TItem">The type of the items on the page.</typeparam>
    /// <returns>The bound pagination handler builder.</returns>
    public static OffsetBasedPaginationHandlerBuilder<TTransformedPage, TItem> OffsetBased<TTransformedPage, TItem>(
        this HttpPaginationHandlerFluentBuilderSetup fluentBuilderSetup)
        => OffsetBasedPaginationHandlerBuilder
            .Create<TTransformedPage, TItem>()
            .WithHttpClient(fluentBuilderSetup.HttpClient);
}
