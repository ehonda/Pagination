using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TwitchPagination;
using TwitchPagination.Authorization;

var services = new ServiceCollection();

// TODO: Maybe use hosting concept: https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host

var configuration = Functions.BuildConfiguration();

// TODO: Fix the issue that we need a paramless ctor here
services
    .AddOptions<ClientData>()
    .Bind(configuration.GetSection("client.json"));

services.AddHttpClient();
services.AddAuthorization();

var provider = services.BuildServiceProvider();

var tokenResponse = await provider.GetRequiredService<IAccessTokenSource>().GetAccessTokenAsync();

var clientData = provider.GetRequiredService<IOptions<ClientData>>().Value;

var client = provider.GetRequiredService<HttpClient>();

// Setup default headers
// TODO: Wire up via dependency injection
client.DefaultRequestHeaders.Authorization = new("Bearer", tokenResponse.AccessToken);
client.DefaultRequestHeaders.Add("Client-Id", clientData.Id);

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

// Search by name example
var queryParam = "name=Fortnite";
var fortniteResponse = await client.GetAsync($"https://api.twitch.tv/helix/games?{queryParam}");
var content = await fortniteResponse.Content.ReadAsStringAsync();
Console.WriteLine(content);

// var doc = await JsonDocument.ParseAsync(await aoe2HdResponse.Content.ReadAsStreamAsync());
// TODO: Why does this not work?
// var writer = new Utf8JsonWriter(Console.OpenStandardOutput());
// doc.WriteTo(writer);
