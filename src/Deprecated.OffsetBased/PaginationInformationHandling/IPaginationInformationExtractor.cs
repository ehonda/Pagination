namespace Deprecated.OffsetBased.PaginationInformationHandling;

/// <summary>
/// Abstraction for extracting pagination information from a response.
/// </summary>
/// <typeparam name="TTransformedPage">
///     The type of the transformed page after pagination information extraction.
/// </typeparam>
[PublicAPI]
public interface IPaginationInformationExtractor<TTransformedPage>
{
    /// <summary>
    /// Extracts the pagination information from the given <paramref name="page"/>.
    /// </summary>
    /// <param name="page">The response to extract the pagination information from.</param>
    /// <param name="cancellationToken">Can be used to cancel the extraction.</param>
    /// <returns>The transformed page with pagination information.</returns>
    Task<TransformedPageWithPaginationInformation<TTransformedPage>> ExtractAsync(
        HttpResponseMessage page,
        CancellationToken cancellationToken = default);
}
