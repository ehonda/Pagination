using Sequential.Composite;

namespace ArrayPaginationHandler;

public class ItemExtractor : IItemExtractor<IEnumerator<int>, int>
{
    public IAsyncEnumerable<int> ExtractItemsAsync(
        IEnumerator<int> context,
        CancellationToken cancellationToken = default)
        => new[] { context.Current }.ToAsyncEnumerable();
}
