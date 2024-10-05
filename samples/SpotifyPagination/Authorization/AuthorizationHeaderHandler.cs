namespace SpotifyPagination.Authorization;

public class AuthorizationHeaderHandler : DelegatingHandler
{
    private readonly IAccessTokenSource _accessTokenSource;

    public AuthorizationHeaderHandler(IAccessTokenSource accessTokenSource)
    {
        _accessTokenSource = accessTokenSource;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var tokenResponse = await _accessTokenSource.GetAccessTokenAsync();
        request.Headers.Authorization = new("Bearer", tokenResponse.AccessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
