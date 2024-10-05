namespace Deprecated.OffsetBased.PageRequestGenerationHandling;

/// <summary>
/// Abstraction for generating HTTP requests for to retrieve a specific page.
/// </summary>
[PublicAPI]
public interface IPageRequestGenerator
{
    /// <summary>
    /// Generates an HTTP request for the given <paramref name="page"/>.
    /// </summary>
    /// <param name="baseUri">The base URI to generate the request for.</param>
    /// <param name="page">The page to generate the request for.</param>
    /// <param name="cancellationToken">Can be used to cancel the generation.</param>
    /// <returns>The generated HTTP request.</returns>
    Task<HttpRequestMessage> GenerateAsync(Uri? baseUri, long page, CancellationToken cancellationToken = default);
}
