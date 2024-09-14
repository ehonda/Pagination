using JetBrains.Annotations;

namespace TwitchPagination;

[UsedImplicitly]
public record TokenResponse(
    string AccessToken,
    long ExpiresIn,
    string TokenType);
