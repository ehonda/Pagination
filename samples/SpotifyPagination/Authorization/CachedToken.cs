using System.Text.Json.Serialization;

namespace SpotifyPagination.Authorization;

public record CachedToken(
    // TODO: Use `NodaTime` with `Instant` here
    [property: JsonPropertyName("acquired_at")]
    DateTimeOffset AcquiredAt,
    [property: JsonPropertyName("access_token_response")]
    AccessTokenResponse AccessTokenResponse)
{
    [JsonIgnore]
    public DateTimeOffset ExpiresAt => AcquiredAt + TimeSpan.FromSeconds(AccessTokenResponse.ExpiresIn);
}
