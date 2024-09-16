using dotenv.net;
using dotenv.net.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace TwitchPagination.Authorization;

public static class ServiceCollectionExtensions
{
    // TODO: Better name
    public static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<ICachedTokenStore, FileCachedTokenStore>(_ =>
        {
            // TODO: Error handling
            DotEnv.Load();
            
            var directory = EnvReader.GetStringValue("PAGINATION_SAMPLE_JSON_DIR_ABSOLUTE_PATH");
            const string file = "cached_token.json";
            var path = Path.Combine(directory, file);
            
            return new(path);
        });

        services.AddKeyedSingleton<IAccessTokenSource, ApiAccessTokenSource>(nameof(ApiAccessTokenSource));
        
        services.AddKeyedSingleton<IAccessTokenSource, CachedAccessTokenSource>(
            nameof(CachedAccessTokenSource),
            (serviceProvider, _) =>
            {
                var cachedTokenStore = serviceProvider.GetRequiredService<ICachedTokenStore>();
                
                var apiAccessTokenSource = serviceProvider.GetRequiredKeyedService<IAccessTokenSource>(
                    nameof(ApiAccessTokenSource));
                
                return new(cachedTokenStore, apiAccessTokenSource);
            });
        
        services.AddSingleton<IAccessTokenSource>(serviceProvider =>
        {
            var cachedAccessTokenSource = serviceProvider.GetRequiredKeyedService<IAccessTokenSource>(
                nameof(CachedAccessTokenSource));
            
            return cachedAccessTokenSource;
        });
        
        return services;
    }
}
