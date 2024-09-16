using System.Net.Http.Json;

namespace TwitchPagination.Authorization;

public class ApiAccessTokenSource : IAccessTokenSource
{
    private readonly ClientData _clientData;
    private readonly HttpClient _httpClient;

    public ApiAccessTokenSource(
        HttpClient httpClient,
        ClientData clientData)
    {
        _clientData = clientData;
        _httpClient = httpClient;
    }

    public async Task<AccessTokenResponse> GetAccessTokenAsync()
    {
        var oauth2TokenResponse = await _httpClient
            .PostAsync(
                // TODO: Pass this as a parameter to the constructor
                "https://id.twitch.tv/oauth2/token",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["client_id"] = _clientData.Id,
                    ["client_secret"] = _clientData.Secret,
                    ["grant_type"] = "client_credentials"
                }));

        // TODO: null handling
        var accessTokenResponse = (await oauth2TokenResponse.Content.ReadFromJsonAsync<AccessTokenResponse>())!;
        
        // TODO: Use `ErrorOr<T>` here
        return accessTokenResponse;
    }
}
