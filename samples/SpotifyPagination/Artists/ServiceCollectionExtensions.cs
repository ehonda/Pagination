using Microsoft.Extensions.DependencyInjection;

namespace SpotifyPagination.Artists;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddArtistsClient(this IServiceCollection services)
    {
        // TODO: Retrieve address from appsettings / parameter string
        services
            .AddHttpClient<ArtistsClient>(client => client.BaseAddress = new("https://api.spotify.com/v1/artists"))
            .ConfigureForApi();

        return services;
    }
}
