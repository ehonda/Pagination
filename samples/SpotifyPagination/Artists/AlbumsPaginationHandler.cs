using OffsetBased;

namespace SpotifyPagination.Artists;

public class AlbumsPaginationHandler : PaginationHandler<GetAlbumsResponse, int, Album>
{
    private readonly ArtistsClient _client;

    public AlbumsPaginationHandler(ArtistsClient client)
    {
        _client = client;
    }

    protected override Task<GetAlbumsResponse> GetPageAsync(GetAlbumsResponse? context,
        CancellationToken cancellationToken = default)
    {
        // TODO: Make limit configurable, make artist configurable
        const int limit = 10;
        var offset = context?.Offset + limit ?? 0;

        return _client.GetAlbums(Ids.GraceJones, limit, offset);
    }

    protected override IAsyncEnumerable<Album> ExtractItemsAsync(GetAlbumsResponse context,
        CancellationToken cancellationToken = default)
        => context.Items.ToAsyncEnumerable();

    protected override Task<int> ExtractOffsetAsync(GetAlbumsResponse context)
        => Task.FromResult(context.Offset);

    protected override Task<int> ExtractTotalAsync(GetAlbumsResponse context)
        => Task.FromResult(context.Total);
}
