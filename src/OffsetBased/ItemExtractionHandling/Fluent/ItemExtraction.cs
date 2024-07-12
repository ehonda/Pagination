using System.Text.Json;

namespace OffsetBased.ItemExtractionHandling.Fluent;

/// <summary>
/// Fluent API for providing an <see cref="IItemExtractor{TTransformedPage,TItem}"/>.
/// </summary>
/// <typeparam name="TTransformedPage">
///     The type of the transformed page after pagination information extraction.
/// </typeparam>
/// <typeparam name="TItem">The type of the items to extract.</typeparam>
[PublicAPI]
public class ItemExtraction<TTransformedPage, TItem>
{
    /// <summary>
    /// Fluent API for providing a <see cref="ItemExtractorViaAsyncFunction{TTransformedPage,TItem}"/>.
    /// </summary>
    /// <param name="extractItems">The async function to extract items from a transformed page.</param>
    /// <returns>The item extractor.</returns>
    public ItemExtractorViaAsyncFunction<TTransformedPage, TItem> ByAsyncFunction(
        Func<TTransformedPage, CancellationToken, IAsyncEnumerable<TItem>> extractItems)
        => new(extractItems);

    /// <summary>
    /// Fluent API for providing a <see cref="ItemExtractorViaFunction{TTransformedPage,TItem}"/>.
    /// </summary>
    /// <param name="extractItems">The function to extract items from a transformed page.</param>
    /// <returns>The item extractor.</returns>
    public ItemExtractorViaFunction<TTransformedPage, TItem> ByFunction(
        Func<TTransformedPage, CancellationToken, IEnumerable<TItem>> extractItems)
        => new(extractItems);

    /// <summary>
    /// Fluent API for providing a
    /// <see cref="ItemExtractorViaAsyncFunctionOnDeserializedJsonContent{TDeserializedJsonContent,TItem}"/>.
    /// </summary>
    /// <param name="extractItems">The async function to extract items from the deserialized JSON content.</param>
    /// <param name="jsonSerializerOptions">The options to use for deserializing the JSON content.</param>
    /// <typeparam name="TDeserializedJsonContent">The type of the deserialized JSON content.</typeparam>
    /// <returns>The item extractor.</returns>
    public ItemExtractorViaAsyncFunctionOnDeserializedJsonContent<TDeserializedJsonContent, TItem>
        ByAsyncFunctionOnDeserializedJsonContent<TDeserializedJsonContent>(
            Func<TDeserializedJsonContent, CancellationToken, IAsyncEnumerable<TItem>> extractItems,
            JsonSerializerOptions? jsonSerializerOptions = null)
        => new(extractItems, jsonSerializerOptions);

    /// <summary>
    /// Fluent API for providing a
    /// <see cref="ItemExtractorViaFunctionOnDeserializedJsonContent{TDeserializedJsonContent,TItem}"/>.
    /// </summary>
    /// <param name="extractItems">The function to extract items from the deserialized JSON content.</param>
    /// <param name="jsonSerializerOptions">The options to use for deserializing the JSON content.</param>
    /// <typeparam name="TDeserializedJsonContent">The type of the deserialized JSON content.</typeparam>
    /// <returns>The item extractor.</returns>
    public ItemExtractorViaFunctionOnDeserializedJsonContent<TDeserializedJsonContent, TItem>
        ByFunctionOnDeserializedJsonContent<TDeserializedJsonContent>(
            Func<TDeserializedJsonContent, IEnumerable<TItem>> extractItems,
            JsonSerializerOptions? jsonSerializerOptions = null)
        => new(extractItems, jsonSerializerOptions);

    /// <summary>
    /// Fluent API for providing a <see cref="RawPagesExtractor{TItem}"/>.
    /// </summary>
    /// <returns>The item extractor.</returns>
    public RawPagesExtractor<TItem> ByRawPages() => new();
}
