using JetBrains.Annotations;

namespace Core.Fluent;

/// <summary>
/// Base class for builders for <see cref="IPaginationHandler{TItem}"/> instances.
/// </summary>
/// <remarks>
/// The builder can be used to either build a <see cref="IPaginationHandler{TItem}"/> instance or to directly build and
/// retrieve all items from the paginated source via <see cref="GetAllItemsAsync"/>, if the <see cref="HttpClient"/> has
/// been set via <see cref="WithHttpClient"/>.
/// </remarks>
/// <typeparam name="TItem">The type of the items to be extracted from the paginated source.</typeparam>
[PublicAPI]
public abstract class PaginationHandlerBuilder<TItem>
{
    /// <summary>
    /// Builds the <see cref="IPaginationHandler{TItem}"/> instance.
    /// </summary>
    /// <returns>The built <see cref="IPaginationHandler{TItem}"/> instance.</returns>
    public abstract IPaginationHandler<TItem> Build();
}
