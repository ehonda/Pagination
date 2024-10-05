using System.Globalization;
using Deprecated.OffsetBased.Utilities;

namespace Deprecated.OffsetBased.PageRequestGenerationHandling;

/// <summary>
/// Query parameters used for offset-based pagination.
/// </summary>
/// <param name="Page">The page to request.</param>
/// <param name="Limit">The limit on the number of items on the page.</param>
[PublicAPI]
public record PaginationParameters(QueryParameter Page, QueryParameter Limit)
{
    /// <summary>
    /// Creates a new instance of <see cref="PaginationParameters"/> by combining the provided
    /// <paramref name="pageParameterValue"/> together with the parameter names and limit parameter value from the
    /// provided <paramref name="configuration"/>.
    /// </summary>
    /// <param name="pageParameterValue">The value of the page parameter.</param>
    /// <param name="configuration">The configuration for the pagination parameters.</param>
    /// <returns>A new instance of <see cref="PaginationParameters"/>.</returns>
    public static PaginationParameters From(long pageParameterValue, PaginationParametersConfiguration configuration)
    {
        var pageParameter = new QueryParameter(
            configuration.PageParameterName,
            pageParameterValue.ToString(CultureInfo.InvariantCulture));

        var limitParameter = new QueryParameter(
            configuration.LimitParameterName,
            configuration.LimitParameterValue.ToString(CultureInfo.InvariantCulture));

        return new(pageParameter, limitParameter);
    }
}
