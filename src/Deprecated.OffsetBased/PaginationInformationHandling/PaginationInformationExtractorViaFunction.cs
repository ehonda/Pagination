namespace Deprecated.OffsetBased.PaginationInformationHandling;

/// <inheritdoc />
/// <remarks>
/// Extraction is realized by the provided function.
/// </remarks>
[PublicAPI]
public class PaginationInformationExtractorViaFunction<TTransformedPage>
    : PaginationInformationExtractorViaAsyncFunction<TTransformedPage>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationInformationExtractorViaFunction{TTransformedPage}"/>
    /// class.
    /// </summary>
    /// <param name="extract">The function to extract pagination information from a response.</param>
    public PaginationInformationExtractorViaFunction(
        Func<HttpResponseMessage, TransformedPageWithPaginationInformation<TTransformedPage>> extract)
        : base((page, _) => Task.FromResult(extract(page)))
    {
    }
}
