using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SpotifyPagination.Authorization;

namespace SpotifyPagination;

public static class HttpClientBuilderExtensions
{
    public static IHttpClientBuilder ConfigureForApi(this IHttpClientBuilder builder)
    {
        // TODO: Should we do this here or not?
        // builder
        //     .Services
        //     .AddAuthorizationServices();

        // TODO: What are the appropriate lifetimes for these?
        builder
            .Services
            .TryAddTransient<AuthorizationHeaderHandler>();

        return builder.AddHttpMessageHandler<AuthorizationHeaderHandler>();
    }
}
