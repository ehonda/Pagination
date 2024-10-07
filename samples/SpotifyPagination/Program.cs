using Microsoft.Extensions.DependencyInjection;
using OffsetBased;
using OffsetBased.Composite;
using SpotifyPagination;
using SpotifyPagination.Artists;
using SpotifyPagination.Artists.Composite;
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

// var handler = new AlbumsPaginationHandler(artistsClient);

// var handler = new PaginationHandlerBuilder<GetAlbumsResponse, int, Album>()
//     .WithPageRetriever(new PageRetriever(artistsClient))
//     .WithIndexDataExtractor(new IndexDataExtractor())
//     .WithItemExtractor(new ItemExtractor())
//     .Build();

var handler = new PaginationHandlerBuilder<GetAlbumsResponse, int, Album>()
    .WithPageRetriever(async (context, _) =>
    {
        const int limit = 10;
        var offset = context?.Offset + limit ?? 0;

        return await artistsClient.GetAlbums(Ids.GraceJones, limit, offset);
    })
    .WithIndexDataExtractor(context => new(context.Offset, context.Total))
    .WithItemExtractor(context => context.Items)
    .Build();

var albums = await handler.GetAllItemsAsync().ToListAsync();

foreach (var album in albums)
{
    Console.WriteLine(album.Name);
}

// var albums = await artistsClient.GetAlbums(Ids.GraceJones, 2, 10);
//
// foreach (var album in albums.Items)
// {
//     Console.WriteLine(album.Name);
// }
