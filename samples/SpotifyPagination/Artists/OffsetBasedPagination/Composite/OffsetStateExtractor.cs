using OffsetBased;
using OffsetBased.Composite.OffsetStateExtractors;

namespace SpotifyPagination.Artists.OffsetBasedPagination.Composite;

public class OffsetStateExtractor : IOffsetStateExtractor<GetAlbumsResponse>
{
    public Task<OffsetState<int>> ExtractOffsetStateAsync(GetAlbumsResponse context,
        CancellationToken cancellationToken = default) 
        => Task.FromResult(new OffsetState<int>(context.Offset, context.Total));
}
