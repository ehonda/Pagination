namespace TwitchPagination.Authorization;

public interface IAccessTokenSource
{
    // TODO: Should we operate directly on the response model or do it a bit cleaner?
    // TODO: Use `ErrorOr<T>` here
    Task<AccessTokenResponse> GetAccessTokenAsync();
}
