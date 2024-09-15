using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TwitchPagination;

[UsedImplicitly]
public record AccessTokenResponse(
    [property: JsonPropertyName("access_token")]
    string AccessToken,
    [property: JsonPropertyName("expires_in")]
    long ExpiresIn,
    [property: JsonPropertyName("token_type")]
    string TokenType);
