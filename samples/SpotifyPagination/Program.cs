using Microsoft.Extensions.DependencyInjection;
using SpotifyPagination;
using SpotifyPagination.Artists;
using SpotifyPagination.Authorization;

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

services.AddArtistsClient();

services.AddAuthorization();

var provider = services.BuildServiceProvider();

// ------------------------------------------------------------------------------------------------------------------ //
//                                                      Usage                                                         //
// ------------------------------------------------------------------------------------------------------------------ //

var gamesClient = provider.GetRequiredService<ArtistsClient>();

// var graceJones = await gamesClient.GetGraceJones();
// Console.WriteLine(graceJones);

var albums = await gamesClient.GetGraceJonesAlbumNames(50);
foreach (var album in albums)
{
    Console.WriteLine(album);
}

// var top500 = await gamesClient.GetAllTopGamesByFunctions(100).Take(500).ToListAsync();
//
// var data = top500
//     .Select(x => x.Name)
//     .Zip(Enumerable.Range(1, 500));
//
// foreach (var (name, index) in data)
// {
//     Console.WriteLine($"{index}\t- {name}");
// }
