using Ardalis.GuardClauses;
using Core;
using OffsetBased.ItemExtractionHandling;
using OffsetBased.PageRequestGenerationHandling;
using OffsetBased.PaginationInformationHandling;
using Sequential;

namespace OffsetBased;

/// <summary>
/// A pagination handler for offset-based pagination.
/// </summary>
/// <typeparam name="TTransformedPage">
///     The type of the transformed page after pagination information extraction.
/// </typeparam>
/// <typeparam name="TItem">The type of the items to extract from the pages.</typeparam>
[PublicAPI]
public class OffsetBasedOriginalPaginationHandler<TTransformedPage, TItem>
    : OriginalPaginationHandler<HttpResponseMessage, TransformedPageWithPaginationInformation<TTransformedPage>, TItem>
{
    private readonly HttpClient _httpClient;
    private readonly IPageRequestGenerator _pageRequestGenerator;
    private readonly IPaginationInformationExtractor<TTransformedPage> _paginationInformationExtractor;
    private readonly IItemExtractor<TTransformedPage, TItem> _itemExtractor;

    /// <summary>
    /// Initializes a new instance of the <see cref="OffsetBasedOriginalPaginationHandler{TTransformedPage,TItem}"/> class.
    /// </summary>
    /// <param name="pageRequestGenerator">Used to generate requests for the pages.</param>
    /// <param name="paginationInformationExtractor">Used to extract pagination information from the pages.</param>
    /// <param name="itemExtractor">Used to extract items from the transformed pages.</param>
    public OffsetBasedOriginalPaginationHandler(
        HttpClient httpClient,
        IPageRequestGenerator pageRequestGenerator,
        IPaginationInformationExtractor<TTransformedPage> paginationInformationExtractor,
        IItemExtractor<TTransformedPage, TItem> itemExtractor)
    {
        _httpClient = httpClient;
        _pageRequestGenerator = pageRequestGenerator;
        _paginationInformationExtractor = paginationInformationExtractor;
        _itemExtractor = itemExtractor;
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> GetFirstPageAsync(
        CancellationToken cancellationToken = default)
    // TODO: Throw If Not Successful
        => await _httpClient
            .SendAsync(
                await _pageRequestGenerator.GenerateAsync(_httpClient.BaseAddress, 1, cancellationToken),
                cancellationToken);

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> GetNextPageAsync(
        TransformedPageWithPaginationInformation<TTransformedPage> currentPage,
        CancellationToken cancellationToken = default)
        // TODO: Throw If Not Successful
        => await _httpClient
            .SendAsync(
                await _pageRequestGenerator.GenerateAsync(
                    _httpClient.BaseAddress,
                    currentPage.PaginationInformation.CurrentPage + 1,
                    cancellationToken),
                cancellationToken);

    /// <inheritdoc />
    protected override Task<bool> NextPageExistsAsync(
        TransformedPageWithPaginationInformation<TTransformedPage> currentPage,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(
            currentPage.PaginationInformation.CurrentPage < currentPage.PaginationInformation.TotalPages);
    }

    /// <inheritdoc />
    protected override async Task<TransformedPageWithPaginationInformation<TTransformedPage>> TransformPageAsync(
        HttpResponseMessage page,
        CancellationToken cancellationToken = default)
    {
        var tuple = await _paginationInformationExtractor.ExtractAsync(page, cancellationToken);

        Guard.Against.NegativeOrZero(tuple.PaginationInformation.CurrentPage);
        Guard.Against.NegativeOrZero(tuple.PaginationInformation.TotalPages);

        return tuple;
    }

    /// <inheritdoc />
    protected override IAsyncEnumerable<TItem> ExtractItemsAsync(
        TransformedPageWithPaginationInformation<TTransformedPage> page,
        CancellationToken cancellationToken = default)
        => _itemExtractor.ExtractAsync(page.TransformedPage, cancellationToken);
}
