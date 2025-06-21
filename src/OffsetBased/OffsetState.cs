namespace EHonda.Pagination.OffsetBased;

/// <summary>
/// Represents the state used for offset-based pagination, including the current offset and the total number of items.
/// </summary>
/// <param name="Offset">The current offset in the paginated resource.</param>
/// <param name="Total">The total number of items in the paginated resource.</param>
/// <typeparam name="TIndex">The type of the index used for pagination (e.g., int, long).</typeparam>
public record OffsetState<TIndex>(TIndex Offset, TIndex Total);
