using dotenv.net;
using dotenv.net.Utilities;
using Microsoft.Extensions.Configuration;

namespace TwitchPagination;

public static class Functions
{
    public static IConfigurationRoot BuildConfiguration()
    {
        DotEnv.Load();
        
        var directory = EnvReader.GetStringValue("PAGINATION_SAMPLE_JSON_DIR_ABSOLUTE_PATH");
        const string file = "client.json";
        var path = Path.Combine(directory, file);
        
        // TODO: Can we make it so we get
        // {
        //     "ClientData": { ... }
        // }
        // here where `{ ... }` is the content of the file
        return new ConfigurationBuilder()
            .AddJsonFile(path)
            .Build();
    }
}
