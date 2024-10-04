using JetBrains.Annotations;

namespace Core.Fluent;

/// <summary>
/// Extensions for <see cref="HttpClient"/> to setup fluent pagination handlers.
/// </summary>
[PublicAPI]
public static class HttpClientExtensions
{
    /// <summary>
    /// Entrypoint for fluent usage of pagination handlers.
    /// </summary>
    /// <param name="httpClient">The http client to use to retrieve the paginated resource.</param>
    /// <returns>The setup via which the pagination handler to use can be selected fluently.</returns>
    public static HttpPaginationHandlerFluentBuilderSetup WithPagination(this HttpClient httpClient)
        => new() { HttpClient = httpClient };
}
