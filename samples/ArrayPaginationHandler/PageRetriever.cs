using Sequential.Composite;

namespace ArrayPaginationHandler;

public class PageRetriever : IPageRetriever<IEnumerator<int>, IEnumerator<int>>
{
    private readonly IEnumerator<int> _firstPage;

    public PageRetriever(IEnumerator<int> firstPage)
    {
        _firstPage = firstPage;
    }

    public Task<IEnumerator<int>> GetAsync(
        IEnumerator<int>? context,
        CancellationToken cancellationToken = default)
        => Task.FromResult(context ?? _firstPage);
}
