using Core;

namespace Sequential.TUnit.Tests;

// TODO: Maybe work with indices
public class EnumeratorPaginationHandler<TItem> : PaginationHandler<IEnumerator<TItem>, TItem>
{
    private readonly IReadOnlyList<TItem> _items;
    private bool _nextPageExists;

    public EnumeratorPaginationHandler(IEnumerable<TItem> items)
    {
        _items = items.ToList();
    }

    protected override Task<IEnumerator<TItem>> GetPageAsync(IEnumerator<TItem>? context,
        CancellationToken cancellationToken = default)
    {
        if (context is null)
        {
            var enumerator = _items.GetEnumerator();
            _nextPageExists = enumerator.MoveNext();

            return Task.FromResult(enumerator);
        }

        _nextPageExists = context.MoveNext();
        return Task.FromResult(context);
    }

    protected override Task<bool> NextPageExistsAsync(IEnumerator<TItem> context,
        CancellationToken cancellationToken = default)
        => Task.FromResult(_nextPageExists);

    protected override IAsyncEnumerable<TItem> ExtractItemsAsync(IEnumerator<TItem> context,
        CancellationToken cancellationToken = default)
    {
        TItem[] items = _nextPageExists ? [context.Current] : [];
        return items.ToAsyncEnumerable();
    }
}

public record Index(int Value);

public class IndexPaginationHandler<TItem> : PaginationHandler<Index, TItem>
{
    private readonly IReadOnlyList<TItem> _items;

    public IndexPaginationHandler(IEnumerable<TItem> items)
    {
        _items = items.ToList();
    }

    protected override Task<Index> GetPageAsync(Index? context, CancellationToken cancellationToken = default)
        => Task.FromResult(context is null ? new(0) : new Index(context.Value + 1));

    protected override Task<bool> NextPageExistsAsync(Index context, CancellationToken cancellationToken = default)
        => Task.FromResult(context.Value < _items.Count - 1);

    protected override IAsyncEnumerable<TItem> ExtractItemsAsync(Index context,
        CancellationToken cancellationToken = default)
    {
        TItem[] items = context.Value < _items.Count ? [_items[context.Value]] : [];
        return items.ToAsyncEnumerable();
    }
}

public class PaginationHandlerTests
{
    public static class ListCases
    {
        public static IEnumerable<List<string>> All() =>
        [
            [],
            ["a"],
            ["a", "b"],
        ];
    }

    public static class HandlerCases
    {
        public delegate IPaginationHandler<string> ConstructorCall(IEnumerable<string> items);

        public static IEnumerable<ConstructorCall> All() =>
        [
            items => new EnumeratorPaginationHandler<string>(items),
            items => new IndexPaginationHandler<string>(items)
        ];
    }

    public static class CombinedCases
    {
        public record Data(List<string> Items, HandlerCases.ConstructorCall ConstructorCall);

        public static IEnumerable<Data> All() => ListCases
            .All()
            .SelectMany(items =>
                HandlerCases
                    .All()
                    .Select(constructor => new Data(items, constructor)));
    }

    [Test]
    [DisplayName("The ListPaginationHandler works")]
    [MethodDataSource(typeof(CombinedCases), nameof(CombinedCases.All))]
    public async Task Handler_works(CombinedCases.Data data)
    {
        // Arrange
        var handler = data.ConstructorCall(data.Items);

        // Act
        var result = await handler.GetAllItemsAsync().ToListAsync();

        // Assert
        await Assert.That(result).IsEquivalentTo(data.Items);
    }
}
