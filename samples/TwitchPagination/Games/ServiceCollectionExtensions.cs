using Microsoft.Extensions.DependencyInjection;

namespace TwitchPagination.Games;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGamesClient(this IServiceCollection services)
    {
        // TODO: Retrieve address from appsettings / parameter string
        services
            .AddHttpClient<GamesClient>(client => client.BaseAddress = new("https://api.twitch.tv/helix/games"))
            .ConfigureForApi();

        return services;
    }
}
