using System.Net.Http.Json;
using System.Text.Json;

namespace TwitchPagination;

public static class Functions
{
    private static TimeSpan Buffer { get; } = TimeSpan.FromSeconds(30);
    
    public static async Task<AccessTokenResponse> GetAccessTokenResponse(ClientData clientData)
    {
        if (await GetCachedAccessTokenResponse() is { } cachedAccessTokenResponse)
        {
            return cachedAccessTokenResponse;
        }
        
        var client = new HttpClient();
        
        var acquiredAt = DateTimeOffset.UtcNow;

        // See: https://dev.twitch.tv/docs/api/get-started/#get-an-oauth-token
        var oauth2TokenResponse = await client
            .PostAsync(
                "https://id.twitch.tv/oauth2/token",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["client_id"] = clientData.Id,
                    ["client_secret"] = clientData.Secret,
                    ["grant_type"] = "client_credentials"
                }));

        var accessTokenResponse = (await oauth2TokenResponse.Content.ReadFromJsonAsync<AccessTokenResponse>())!;
        
        var cachedToken = new CachedToken(acquiredAt, accessTokenResponse);
        
        // TODO: Make this go to the project directory
        await JsonSerializer.SerializeAsync(File.OpenWrite("token.json"), cachedToken);
        
        return accessTokenResponse;
    }
    
    // TODO: Use ErrorOr<T> here
    private static async Task<AccessTokenResponse?> GetCachedAccessTokenResponse()
    {
        if (File.Exists("token.json") is false)
        {
            return null;
        }

        var cachedToken = (await JsonSerializer.DeserializeAsync<CachedToken>(File.OpenRead("token.json")))!;

        var expiresAt = cachedToken.AcquiredAt + TimeSpan.FromSeconds(cachedToken.AccessTokenResponse.ExpiresIn);
        var cutoff = DateTimeOffset.UtcNow - Buffer;
        
        if (expiresAt < cutoff)
        {
            return null;
        }

        return cachedToken.AccessTokenResponse;
    }
}
