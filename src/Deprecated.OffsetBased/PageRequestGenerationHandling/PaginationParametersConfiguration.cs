namespace Deprecated.OffsetBased.PageRequestGenerationHandling;

/// <summary>
/// Configuration for offset-based pagination query parameters.
/// </summary>
/// <param name="PageParameterName">The name of the page parameter.</param>
/// <param name="LimitParameterName">The name of the limit parameter.</param>
/// <param name="LimitParameterValue">The value of the limit parameter.</param>
[PublicAPI]
public record PaginationParametersConfiguration(
    string PageParameterName,
    string LimitParameterName,
    long LimitParameterValue);
