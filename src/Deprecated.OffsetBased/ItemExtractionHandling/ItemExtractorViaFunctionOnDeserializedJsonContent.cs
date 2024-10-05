using System.Text.Json;

namespace Deprecated.OffsetBased.ItemExtractionHandling;

/// <inheritdoc />
/// <remarks>
/// Extraction is realized by deserializing the JSON content of the page and then extracting the items via the provided
/// extraction function.
/// </remarks>
[PublicAPI]
public class ItemExtractorViaFunctionOnDeserializedJsonContent<TDeserializedJsonContent, TItem>
    : ItemExtractorViaAsyncFunctionOnDeserializedJsonContent<TDeserializedJsonContent, TItem>
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="ItemExtractorViaFunctionOnDeserializedJsonContent{TDeserializedJsonContent, TItem}"/> class.
    /// </summary>
    /// <param name="extractItems">The function to extract items from the deserialized JSON content.</param>
    /// <param name="jsonSerializerOptions">The options to use for deserializing the JSON content.</param>
    public ItemExtractorViaFunctionOnDeserializedJsonContent(
        Func<TDeserializedJsonContent, IEnumerable<TItem>> extractItems,
        JsonSerializerOptions? jsonSerializerOptions = null)
        : base(
            (deserialized, _) => extractItems(deserialized).ToAsyncEnumerable(),
            jsonSerializerOptions)
    {
    }
}
