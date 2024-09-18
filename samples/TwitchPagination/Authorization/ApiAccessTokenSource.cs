using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace TwitchPagination.Authorization;

public class ApiAccessTokenSource : IAccessTokenSource
{
    private readonly IOptions<ClientData> _clientData;
    private readonly HttpClient _httpClient;

    public ApiAccessTokenSource(
        HttpClient httpClient,
        IOptions<ClientData> clientData)
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
                    ["client_id"] = _clientData.Value.Id,
                    ["client_secret"] = _clientData.Value.Secret,
                    ["grant_type"] = "client_credentials"
                }));

        // TODO: null handling
        var accessTokenResponse = (await oauth2TokenResponse.Content.ReadFromJsonAsync<AccessTokenResponse>())!;
        
        // TODO: Use `ErrorOr<T>` here
        return accessTokenResponse;
    }
}
