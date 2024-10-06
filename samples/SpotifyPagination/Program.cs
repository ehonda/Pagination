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

var artistsClient = provider.GetRequiredService<ArtistsClient>();

var albums = await artistsClient.GetAlbums(Ids.GraceJones, 2, 10);

foreach (var album in albums.Items)
{
    Console.WriteLine(album.Name);
}
