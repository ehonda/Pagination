namespace Deprecated.OffsetBased.PaginationInformationHandling.Fluent;

/// <summary>
/// Fluent API for providing an <see cref="IPaginationInformationExtractor{TTransformedPage}"/>.
/// </summary>
/// <typeparam name="TTransformedPage">
///     The type of the transformed page after pagination information extraction.
/// </typeparam>
[PublicAPI]
public class PaginationInformationExtraction<TTransformedPage>
{
    /// <summary>
    /// Fluent API for providing a <see cref="PaginationInformationExtractorViaFunction{TTransformedPage}"/>.
    /// </summary>
    /// <param name="extract">The function to extract pagination information from a response.</param>
    /// <returns>The pagination information extractor.</returns>
    public PaginationInformationExtractorViaFunction<TTransformedPage> ByFunction(
        Func<HttpResponseMessage, TransformedPageWithPaginationInformation<TTransformedPage>> extract)
        => new(extract);

    /// <summary>
    /// Fluent API for providing a <see cref="PaginationInformationExtractorViaAsyncFunction{TTransformedPage}"/>.
    /// </summary>
    /// <param name="extractAsync">The async function to extract pagination information from a response.</param>
    /// <returns>The pagination information extractor.</returns>
    public PaginationInformationExtractorViaAsyncFunction<TTransformedPage> ByAsyncFunction(
        Func<HttpResponseMessage, CancellationToken, Task<TransformedPageWithPaginationInformation<TTransformedPage>>> extractAsync)
        => new(extractAsync);
}
