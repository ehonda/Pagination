namespace OffsetBased.PaginationInformationHandling;

/// <inheritdoc />
/// <remarks>
/// Extraction is realized by the provided async extraction function.
/// </remarks>
[PublicAPI]
public class PaginationInformationExtractorViaAsyncFunction<TTransformedPage>
    : IPaginationInformationExtractor<TTransformedPage>
{
    private readonly Func<
        HttpResponseMessage,
        CancellationToken,
        Task<TransformedPageWithPaginationInformation<TTransformedPage>>> _extractAsync;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationInformationExtractorViaAsyncFunction{TTransformedPage}"/>
    /// class.
    /// </summary>
    /// <param name="extractAsync">The async function to extract pagination information from a response.</param>
    public PaginationInformationExtractorViaAsyncFunction(
        Func<
            HttpResponseMessage,
            CancellationToken,
            Task<TransformedPageWithPaginationInformation<TTransformedPage>>> extractAsync)
    {
        _extractAsync = extractAsync;
    }

    /// <inheritdoc />
    public Task<TransformedPageWithPaginationInformation<TTransformedPage>> ExtractAsync(
        HttpResponseMessage page,
        CancellationToken cancellationToken = default)
        => _extractAsync(page, cancellationToken);
}
