using Sequential.Composite.PageRetrievers;

namespace SpotifyPagination.Artists.OffsetBasedPagination.Composite;

public class PageRetriever : IPageRetriever<GetAlbumsResponse>
{
    private readonly ArtistsClient _client;

    public PageRetriever(ArtistsClient client)
    {
        _client = client;
    }

    public Task<GetAlbumsResponse> GetAsync(GetAlbumsResponse? context,
        CancellationToken cancellationToken = default)
    {
        // TODO: Make limit configurable, make artist configurable
        const int limit = 10;
        var offset = context?.Offset + limit ?? 0;

        return _client.GetAlbums(Ids.GraceJones, limit, offset);
    }
}
