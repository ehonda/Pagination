using Microsoft.Extensions.DependencyInjection;
using TwitchPagination;
using TwitchPagination.Authorization;
using TwitchPagination.Games;

// ------------------------------------------------------------------------------------------------------------------ //
//                                                      Setup                                                         //
// ------------------------------------------------------------------------------------------------------------------ //

var services = new ServiceCollection();

// TODO: Maybe use hosting concept: https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host

var configuration = Functions.BuildConfiguration();

services
    .AddOptions<ClientData>()
    .Bind(configuration.GetSection(ClientData.ConfigurationSectionName));

services
    .AddHttpClient(nameof(HttpClient))
    .ConfigureForApi();

services.AddGamesClient();

services.AddAuthorization();

var provider = services.BuildServiceProvider();

// ------------------------------------------------------------------------------------------------------------------ //
//                                                      Usage                                                         //
// ------------------------------------------------------------------------------------------------------------------ //

var gamesClient = provider.GetRequiredService<GamesClient>();

var data = (await gamesClient.GetTopGamesNames(100))
    .Zip(Enumerable.Range(1, 100));

foreach (var (name, index) in data)
{
    Console.WriteLine($"{index}\t- {name}");
}
