using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace OffsetBased.ItemExtractionHandling;

/// <inheritdoc />
/// <remarks>
/// Extraction is realized by deserializing the JSON content of the page and then extracting the items via the provided
/// async extraction function.
/// </remarks>
[PublicAPI]
public class ItemExtractorViaAsyncFunctionOnDeserializedJsonContent<TDeserializedJsonContent, TItem>
    : ItemExtractorViaAsyncFunction<HttpResponseMessage, TItem>
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ItemExtractorViaAsyncFunctionOnDeserializedJsonContent{TDeserializedJsonContent, TItem}"/> class.
    /// </summary>
    /// <param name="extractItems">The function to extract items from the deserialized JSON content.</param>
    /// <param name="jsonSerializerOptions">The options to use for deserializing the JSON content.</param>
    public ItemExtractorViaAsyncFunctionOnDeserializedJsonContent(
        Func<TDeserializedJsonContent, CancellationToken, IAsyncEnumerable<TItem>> extractItems,
        JsonSerializerOptions? jsonSerializerOptions = null)
        : base(CreateExtractItems(extractItems, jsonSerializerOptions))
    {
    }

    private static Func<HttpResponseMessage, CancellationToken, IAsyncEnumerable<TItem>> CreateExtractItems(
        Func<TDeserializedJsonContent, CancellationToken, IAsyncEnumerable<TItem>> extractItems,
        JsonSerializerOptions? jsonSerializerOptions)
        => (page, cancellationToken) => ExtractItemsLocalAsync(
            extractItems, jsonSerializerOptions, page, cancellationToken);

    // We can't inline this into `CreateExtractItems` because the method is async and we can't use `yield return` in an
    // async method.
    private static async IAsyncEnumerable<TItem> ExtractItemsLocalAsync(
        Func<TDeserializedJsonContent, CancellationToken, IAsyncEnumerable<TItem>> extractItems,
        JsonSerializerOptions? jsonSerializerOptions,
        HttpResponseMessage httpResponseMessage,
        [EnumeratorCancellation] CancellationToken localCancellationToken)
    {
        var deserialized = await DeserializeJsonContentAsync(
            httpResponseMessage, jsonSerializerOptions, localCancellationToken);

        await foreach (var item in extractItems(deserialized, localCancellationToken))
        {
            yield return item;
        }
    }

    private static async Task<TDeserializedJsonContent> DeserializeJsonContentAsync(
        HttpResponseMessage page,
        JsonSerializerOptions? jsonSerializerOptions,
        CancellationToken cancellationToken = default)
    {
        var deserializedPage = await page.Content.ReadFromJsonAsync<TDeserializedJsonContent>(
            jsonSerializerOptions,
            cancellationToken);

        if (deserializedPage is null)
        {
            throw new InvalidOperationException(
                $"Could not deserialize the page to an instance of {nameof(TItem)}.");
        }

        return deserializedPage;
    }
}
