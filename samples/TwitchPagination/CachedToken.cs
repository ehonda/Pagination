using System.Text.Json.Serialization;

namespace TwitchPagination;

public record CachedToken(
    // TODO: Use `NodaTime` with `Instant` here
    [property: JsonPropertyName("acquiredAt")]
    DateTimeOffset AcquiredAt,
    [property: JsonPropertyName("accessTokenResponse")]
    AccessTokenResponse AccessTokenResponse);
