using OffsetBased.Composite;

namespace SpotifyPagination.Artists.OffsetBasedPagination.Composite;

public class AlbumsPaginationHandler : PaginationHandler<GetAlbumsResponse, Album>
{
    public AlbumsPaginationHandler(ArtistsClient client)
        : base(new PageRetriever(client), new IndexDataExtractor(), new ItemExtractor())
    {
    }
}
