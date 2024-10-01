using Microsoft.Extensions.DependencyInjection;
using TwitchPagination;
using TwitchPagination.Authorization;
using TwitchPagination.Games;

// ------------------------------------------------------------------------------------------------------------------ //
//                                                      Setup                                                         //
// ------------------------------------------------------------------------------------------------------------------ //

var services = new ServiceCollection();

// TODO: Maybe use hosting concept: https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host
// TODO: Set up logging

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
// TODO: Improve ux, fluent builder like for this
var handler = new TwitchPagination.Games.V4.Composite.TopGamesPaginationHandler(gamesClient);

var top500 = await handler.GetAllItemsAsync().Take(500).ToListAsync();

var data = top500
    .Select(x => x.Name)
    .Zip(Enumerable.Range(1, 500));

foreach (var (name, index) in data)
{
    Console.WriteLine($"{index}\t- {name}");
}
