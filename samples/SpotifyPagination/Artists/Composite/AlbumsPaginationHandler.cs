using OffsetBased.Composite;

namespace SpotifyPagination.Artists.Composite;

public class AlbumsPaginationHandler : PaginationHandler<GetAlbumsResponse, int, Album>
{
    public AlbumsPaginationHandler(ArtistsClient client)
        : base(new PageRetriever(client), new IndexDataExtractor(), new ItemExtractor())
    {
    }
}
