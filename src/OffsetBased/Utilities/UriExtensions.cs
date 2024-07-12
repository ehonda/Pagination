using System.Web;

namespace OffsetBased.Utilities;

/// <summary>
/// Extension methods for <see cref="Uri"/>.
/// </summary>
[PublicAPI]
public static class UriExtensions
{
    /// <summary>
    /// Adds to or updates the query parameters of a copy of <paramref name="uri"/>. Existing keys will be updated with
    /// the new values specified via <paramref name="parameters"/> and non-existing keys will be added.
    /// </summary>
    /// <remarks>
    /// The original <paramref name="uri"/> is not modified, instead a new <see cref="Uri"/> instance is returned.
    /// </remarks>
    /// <param name="uri">The <see cref="Uri"/> to add to or update the query parameters of.</param>
    /// <param name="parameters">The query parameters to add or update.</param>
    /// <returns>A new <see cref="Uri"/> instance with the query parameters added or updated.</returns>
    [Pure]
    public static Uri WithAddedOrUpdatedQueryParameters(this Uri uri, params QueryParameter[] parameters)
    {
        var query = HttpUtility.ParseQueryString(uri.Query);

        foreach (var (name, value) in parameters)
        {
            query[name] = value;
        }

        var builder = new UriBuilder(uri) { Query = query.ToString() };

        return builder.Uri;
    }
}
