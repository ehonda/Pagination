using Microsoft.Extensions.DependencyInjection;
using TwitchPagination;
using TwitchPagination.Authorization;
using TwitchPagination.Games;

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

// // IGDB ID: https://www.igdb.com/games/age-of-empires-ii-hd-edition
// var queryParam = "igdb_id=2950";
// var aoe2HdResponse = await client.GetAsync($"https://api.twitch.tv/helix/games?{queryParam}");
//
// var content = await aoe2HdResponse.Content.ReadAsStringAsync();
//
// Console.WriteLine(content);

// var topGamesResponse = await client.GetAsync("https://api.twitch.tv/helix/games?first=10");
// var content = await topGamesResponse.Content.ReadAsStringAsync();
// Console.WriteLine(content);

// var factory = provider.GetRequiredService<IHttpClientFactory>();
// var client = factory.CreateClient(nameof(HttpClient));

// Search by name example

var gamesClient = provider.GetRequiredService<GamesClient>();

var fortnite = await gamesClient.GetFortnite();
Console.WriteLine(fortnite);
