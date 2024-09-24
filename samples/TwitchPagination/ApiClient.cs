using Microsoft.Extensions.Options;
using TwitchPagination.Authorization;

namespace TwitchPagination;

// Cheap-ass typed client because we want to add auth and we can't do it in `AddHttpClient` directly since it does not
// support `async` setup, which we need in order to get the access token
public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<ClientData> _clientDataOptions;
    private readonly IAccessTokenSource _accessTokenSource;

    private bool _accessTokenAdded;

    public ApiClient(
        HttpClient httpClient,
        IOptions<ClientData> clientDataOptions,
        IAccessTokenSource accessTokenSource)
    {
        _httpClient = httpClient;
        _clientDataOptions = clientDataOptions;
        _accessTokenSource = accessTokenSource;
    }

    public async Task<HttpClient> GetClient()
    {
        // TODO: Don't just do this once
        if (_accessTokenAdded is false)
        {
            var tokenResponse = await _accessTokenSource.GetAccessTokenAsync();
            
            _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", tokenResponse.AccessToken);
            _httpClient.DefaultRequestHeaders.Add("Client-Id", _clientDataOptions.Value.Id);
            
            _accessTokenAdded = true;
        }
        
        return _httpClient;
    }
}
