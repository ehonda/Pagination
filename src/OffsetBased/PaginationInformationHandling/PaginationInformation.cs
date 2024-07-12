namespace OffsetBased.PaginationInformationHandling;

/// <summary>
/// Represents the information required for pagination in a response.
/// </summary>
/// <param name="CurrentPage">The current page.</param>
/// <param name="TotalPages">The total number of pages.</param>
[PublicAPI]
public record PaginationInformation(long CurrentPage, long TotalPages);
