using dotenv.net;
using dotenv.net.Utilities;
using Microsoft.Extensions.Configuration;

namespace TwitchPagination;

public static class Functions
{
    public static IConfigurationRoot BuildConfiguration()
    {
        DotEnv.Load();
        
        var directory = EnvReader.GetStringValue("PAGINATION_SAMPLE_CONFIGURATION_DIR");
        var path = Path.Combine(directory, "client.json");
        
        return new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();
    }
}
