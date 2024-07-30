using Ardalis.GuardClauses;
using Core;
using Core.Fluent;
using OffsetBased.ItemExtractionHandling;
using OffsetBased.ItemExtractionHandling.Fluent;
using OffsetBased.PageRequestGenerationHandling;
using OffsetBased.PageRequestGenerationHandling.Fluent;
using OffsetBased.PaginationInformationHandling;
using OffsetBased.PaginationInformationHandling.Fluent;

namespace OffsetBased.Fluent;

// TODO: Clean up the structure and all this mess, comment on how this works
public class OffsetBasedPaginationHandlerBuilder<TTransformedPage, TItem>
    : OffsetBasedPaginationHandlerBuilder<
        OffsetBasedPaginationHandlerBuilder<TTransformedPage, TItem>,
        TTransformedPage,
        TItem>;

/// <inheritdoc />
/// <typeparam name="TTransformedPage">
/// The type of the transformed page after pagination information extraction.
/// </typeparam>
// CS1712: Type parameter has no matching typeparam tag in the XML comment (but other type parameters do)
// Justification: False positive, since we inheritdoc.
[PublicAPI]
#pragma warning disable CS1712 // Type parameter has no matching typeparam tag in the XML comment (but other type parameters do)
public class OffsetBasedPaginationHandlerBuilder<TDerivedBuilder, TTransformedPage, TItem>
    : PaginationHandlerBuilderWithBoundHttpClient<TDerivedBuilder, TItem>
    where TDerivedBuilder : OffsetBasedPaginationHandlerBuilder<TDerivedBuilder, TTransformedPage, TItem>
#pragma warning restore CS1712 // Type parameter has no matching typeparam tag in the XML comment (but other type parameters do)
{
    private IPageRequestGenerator? _pageRequestGenerator;

    private IItemExtractor<TTransformedPage, TItem>? _itemExtractor;

    private IPaginationInformationExtractor<TTransformedPage>? _paginationInformationExtractor;

    /// <summary>
    /// Sets the <see cref="IPageRequestGenerator"/> to use for generating the page requests.
    /// </summary>
    /// <param name="pageRequestGenerator">The <see cref="IPageRequestGenerator"/> to use.</param>
    /// <returns>The modified builder instance.</returns>
    public TDerivedBuilder WithPageRequestGeneration(IPageRequestGenerator pageRequestGenerator)
    {
        _pageRequestGenerator = pageRequestGenerator;
        return (TDerivedBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="IPageRequestGenerator"/> to use for generating the page requests.
    /// </summary>
    /// <param name="pageRequestGeneration">
    ///     A function to create the <see cref="IPageRequestGenerator"/> from a <see cref="PageRequestGeneration"/>.
    /// </param>
    /// <returns>The modified builder instance.</returns>
    public TDerivedBuilder WithPageRequestGeneration(
        Func<PageRequestGeneration, IPageRequestGenerator> pageRequestGeneration)
    {
        _pageRequestGenerator = pageRequestGeneration(new());
        return (TDerivedBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="IPaginationInformationExtractor{TTransformedPage}"/> to use for extracting the pagination
    /// information from the page.
    /// </summary>
    /// <param name="paginationInformationExtractor">
    ///     The <see cref="IPaginationInformationExtractor{TTransformedPage}"/> to use.
    /// </param>
    /// <returns>The modified builder instance.</returns>
    public TDerivedBuilder WithPaginationInformationExtraction(
        IPaginationInformationExtractor<TTransformedPage> paginationInformationExtractor)
    {
        _paginationInformationExtractor = paginationInformationExtractor;
        return (TDerivedBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="IPaginationInformationExtractor{TTransformedPage}"/> to use for extracting the pagination.
    /// </summary>
    /// <param name="paginationInformationExtraction">
    ///     A function to create the <see cref="IPaginationInformationExtractor{TTransformedPage}"/> from a
    ///     <see cref="PaginationInformationExtraction{TTransformedPage}"/>.
    /// </param>
    /// <returns>The modified builder instance.</returns>
    public TDerivedBuilder WithPaginationInformationExtraction(
        Func<PaginationInformationExtraction<TTransformedPage>, IPaginationInformationExtractor<TTransformedPage>>
            paginationInformationExtraction)
    {
        _paginationInformationExtractor = paginationInformationExtraction(new());
        return (TDerivedBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="IItemExtractor{TTransformedPage, TItem}"/> to use for extracting the items from the page.
    /// </summary>
    /// <param name="itemExtractor">The <see cref="IItemExtractor{TTransformedPage, TItem}"/> to use.</param>
    /// <returns>The modified builder instance.</returns>
    public TDerivedBuilder WithItemExtraction(IItemExtractor<TTransformedPage, TItem> itemExtractor)
    {
        _itemExtractor = itemExtractor;
        return (TDerivedBuilder)this;
    }

    /// <summary>
    /// Sets the <see cref="IItemExtractor{TTransformedPage, TItem}"/> to use for extracting the items from the page.
    /// </summary>
    /// <param name="itemExtraction">
    ///     A function to create the <see cref="IItemExtractor{TTransformedPage, TItem}"/> from an
    ///     <see cref="ItemExtraction{TTransformedPage, TItem}"/>.
    /// </param>
    /// <returns>The modified builder instance.</returns>
    public TDerivedBuilder WithItemExtraction(
        Func<ItemExtraction<TTransformedPage, TItem>, IItemExtractor<TTransformedPage, TItem>> itemExtraction)
    {
        _itemExtractor = itemExtraction(new());
        return (TDerivedBuilder)this;
    }

    /// <inheritdoc />
    /// <exception cref="NullReferenceException">If any of the components have not been set.</exception>
    public override IPaginationHandler<TItem> Build()
    {
        Guard.Against.Null(
            _pageRequestGenerator,
            message: ComponentNotSetMessage(nameof(_pageRequestGenerator), nameof(WithPageRequestGeneration)));

        Guard.Against.Null(
            _paginationInformationExtractor,
            message: ComponentNotSetMessage(
                nameof(_paginationInformationExtractor),
                nameof(WithPaginationInformationExtraction)));

        Guard.Against.Null(
            _itemExtractor,
            message: ComponentNotSetMessage(nameof(_itemExtractor), nameof(WithItemExtraction)));

        return new OffsetBasedPaginationHandler<TTransformedPage, TItem>(
            _pageRequestGenerator,
            _paginationInformationExtractor,
            _itemExtractor);
    }

    private static string ComponentNotSetMessage(string componentName, string methodName)
        => $"The {componentName} must be set via {methodName} when using {nameof(Build)} "
           + $"with a {nameof(OffsetBasedPaginationHandlerBuilder<TDerivedBuilder, TTransformedPage, TItem>)}.";
}
