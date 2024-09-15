using System.Text.Json.Serialization;

namespace TwitchPagination;

public record ClientData(
    [property: JsonPropertyName("id")]
    string Id,
    [property: JsonPropertyName("secret")]
    string Secret);
