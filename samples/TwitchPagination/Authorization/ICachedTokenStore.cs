namespace TwitchPagination.Authorization;

public interface ICachedTokenStore
{
    // TODO: Error handling via ErrorOr<T>
    Task PutAsync(CachedToken cachedToken);
    
    // TODO: Don't use null to indicate failure
    Task<CachedToken?> GetAsync();
}
