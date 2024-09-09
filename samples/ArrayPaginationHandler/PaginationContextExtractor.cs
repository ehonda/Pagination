using Sequential.Composite;

namespace ArrayPaginationHandler;

public class PaginationContextExtractor : IPaginationContextExtractor<IEnumerator<int>, IEnumerator<int>>
{
    public Task<IEnumerator<int>> ExtractAsync(
        IEnumerator<int> page,
        CancellationToken cancellationToken = default)
        => Task.FromResult(page);
}
