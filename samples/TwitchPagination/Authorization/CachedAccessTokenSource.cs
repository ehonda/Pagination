namespace TwitchPagination.Authorization;

public class CachedAccessTokenSource : IAccessTokenSource
{
    private readonly ICachedTokenStore _cachedTokenStoreStore;
    private readonly IAccessTokenSource _accessTokenSource;

    // TODO: Make configurable
    private static TimeSpan Buffer { get; } = TimeSpan.FromSeconds(30);

    public CachedAccessTokenSource(
        ICachedTokenStore cachedTokenStoreStore,
        IAccessTokenSource accessTokenSource)
    {
        _cachedTokenStoreStore = cachedTokenStoreStore;
        _accessTokenSource = accessTokenSource;
    }

    public async Task<AccessTokenResponse> GetAccessTokenAsync()
    {
        // If the token has expired we need to refresh our store
        var cachedToken = await _cachedTokenStoreStore.GetAsync();

        if (cachedToken is null || cachedToken.ExpiresAt < DateTimeOffset.UtcNow - Buffer)
        {
            var accessTokenResponse = await _accessTokenSource.GetAccessTokenAsync();

            await _cachedTokenStoreStore.PutAsync(new(
                DateTimeOffset.UtcNow,
                accessTokenResponse));

            return accessTokenResponse;
        }

        return cachedToken.AccessTokenResponse;
    }
}
