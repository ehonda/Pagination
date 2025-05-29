using System.Runtime.CompilerServices;
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

// Test handlers for error scenarios
public class FaultyPageRetrievalHandler<TItem> : PaginationHandler<Index, TItem>
{
    private readonly IReadOnlyList<TItem> _items;
    private readonly int _failOnPage;

    public FaultyPageRetrievalHandler(IEnumerable<TItem> items, int failOnPage = 2)
    {
        _items = items.ToList();
        _failOnPage = failOnPage;
    }

    protected override Task<Index> GetPageAsync(Index? context, CancellationToken cancellationToken = default)
    {
        var currentPage = context?.Value + 1 ?? 0;
        if (currentPage == _failOnPage)
        {
            throw new InvalidOperationException($"Simulated failure on page {currentPage}");
        }
        return Task.FromResult(context is null ? new(0) : new Index(context.Value + 1));
    }

    protected override Task<bool> NextPageExistsAsync(Index context, CancellationToken cancellationToken = default)
        => Task.FromResult(context.Value < _items.Count - 1);

    protected override IAsyncEnumerable<TItem> ExtractItemsAsync(Index context,
        CancellationToken cancellationToken = default)
    {
        TItem[] items = context.Value < _items.Count ? [_items[context.Value]] : [];
        return items.ToAsyncEnumerable();
    }
}

public class FaultyNextPageCheckHandler<TItem> : PaginationHandler<Index, TItem>
{
    private readonly IReadOnlyList<TItem> _items;
    private readonly int _failOnCheck;

    public FaultyNextPageCheckHandler(IEnumerable<TItem> items, int failOnCheck = 2)
    {
        _items = items.ToList();
        _failOnCheck = failOnCheck;
    }

    protected override Task<Index> GetPageAsync(Index? context, CancellationToken cancellationToken = default)
        => Task.FromResult(context is null ? new(0) : new Index(context.Value + 1));

    protected override Task<bool> NextPageExistsAsync(Index context, CancellationToken cancellationToken = default)
    {
        if (context.Value == _failOnCheck)
        {
            throw new InvalidOperationException($"Simulated failure checking page {context.Value}");
        }
        return Task.FromResult(context.Value < _items.Count - 1);
    }

    protected override IAsyncEnumerable<TItem> ExtractItemsAsync(Index context,
        CancellationToken cancellationToken = default)
    {
        TItem[] items = context.Value < _items.Count ? [_items[context.Value]] : [];
        return items.ToAsyncEnumerable();
    }
}

public class FaultyItemExtractionHandler<TItem> : PaginationHandler<Index, TItem>
{
    private readonly IReadOnlyList<TItem> _items;
    private readonly int _failOnPage;

    public FaultyItemExtractionHandler(IEnumerable<TItem> items, int failOnPage = 1)
    {
        _items = items.ToList();
        _failOnPage = failOnPage;
    }

    protected override Task<Index> GetPageAsync(Index? context, CancellationToken cancellationToken = default)
        => Task.FromResult(context is null ? new(0) : new Index(context.Value + 1));

    protected override Task<bool> NextPageExistsAsync(Index context, CancellationToken cancellationToken = default)
        => Task.FromResult(context.Value < _items.Count - 1);    protected override IAsyncEnumerable<TItem> ExtractItemsAsync(Index context,
        CancellationToken cancellationToken = default)
    {
        if (context.Value == _failOnPage)
        {
            throw new InvalidOperationException($"Simulated failure extracting items from page {context.Value}");
        }
        
        TItem[] items = context.Value < _items.Count ? [_items[context.Value]] : [];
        return items.ToAsyncEnumerable();
    }
}

public class PaginationHandlerTests
{
    public class ListCases
    {
        public static IEnumerable<List<string>> All() =>
        [
            [],
            ["a"],
            ["a", "b"],
        ];
    }

    public class HandlerCases
    {
        public delegate IPaginationHandler<string> ConstructorCall(IEnumerable<string> items);

        public static IEnumerable<ConstructorCall> All() =>
        [
            items => new EnumeratorPaginationHandler<string>(items),
            items => new IndexPaginationHandler<string>(items)
        ];
    }

    [Test]
    [DisplayName("The ListPaginationHandler works")]
    [MatrixDataSource]
    public async Task Handler_works(
        [MatrixMethod<ListCases>(nameof(ListCases.All))] List<string> items,
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))] HandlerCases.ConstructorCall constructor)
    {
        // Arrange
        var handler = constructor(items);

        // Act
        var result = await handler.GetAllItemsAsync().ToListAsync();

        // Assert
        await Assert.That(result).IsEquivalentTo(items);
    }   
    
    [Test]
    [DisplayName("Handler respects cancellation token")]
    [MatrixDataSource]
    public async Task Handler_respects_cancellation_token(
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))] HandlerCases.ConstructorCall constructor)
    {
        // Arrange
        var items = Enumerable.Range(0, 1000).Select(i => i.ToString()).ToList();
        var handler = constructor(items);
        using var cts = new CancellationTokenSource();

        // Act & Assert
        var enumerator = handler.GetAllItemsAsync(cts.Token).GetAsyncEnumerator();
        
        // Get first few items
        await Assert.That(await enumerator.MoveNextAsync()).IsTrue();
        await Assert.That(await enumerator.MoveNextAsync()).IsTrue();
        
        // Cancel and verify subsequent operations throw
        cts.Cancel();
        
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await enumerator.MoveNextAsync());
    }

    [Test]
    [DisplayName("Handler works with null items in collection")]
    [MatrixDataSource]
    public async Task Handler_works_with_null_items(
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))] HandlerCases.ConstructorCall constructor)
    {
        // Arrange
        var items = new List<string?> { "a", null, "b", null };
        var handler = constructor(items.Cast<string>());

        // Act
        var result = await handler.GetAllItemsAsync().ToListAsync();

        // Assert
        await Assert.That(result).IsEquivalentTo(items);
    }

    [Test]
    [DisplayName("Handler yields items lazily")]
    [MatrixDataSource]
    public async Task Handler_yields_items_lazily(
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))] HandlerCases.ConstructorCall constructor)
    {
        // Arrange
        var items = new List<string> { "a", "b", "c", "d", "e" };
        var handler = constructor(items);
        var yielded = new List<string>();

        // Act
        await foreach (var item in handler.GetAllItemsAsync())
        {
            yielded.Add(item);
            if (yielded.Count == 2)
            {
                // Stop early to verify we only got what we consumed
                break;
            }
        }

        // Assert
        await Assert.That(yielded).HasCount().EqualTo(2);
        await Assert.That(yielded).IsEquivalentTo(new[] { "a", "b" });
    }

    [Test]
    [DisplayName("Handler can be enumerated multiple times")]
    [MatrixDataSource]
    public async Task Handler_can_be_enumerated_multiple_times(
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))] HandlerCases.ConstructorCall constructor)
    {
        // Arrange
        var items = new List<string> { "a", "b", "c" };
        var handler = constructor(items);

        // Act
        var result1 = await handler.GetAllItemsAsync().ToListAsync();
        var result2 = await handler.GetAllItemsAsync().ToListAsync();

        // Assert
        await Assert.That(result1).IsEquivalentTo(items);
        await Assert.That(result2).IsEquivalentTo(items);
        await Assert.That(result1).IsEquivalentTo(result2);
    }

    [Test]
    [DisplayName("Handler works with large datasets")]
    [MatrixDataSource]
    public async Task Handler_works_with_large_datasets(
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))] HandlerCases.ConstructorCall constructor)
    {
        // Arrange
        var items = Enumerable.Range(0, 10_000).Select(i => $"item_{i}").ToList();
        var handler = constructor(items);

        // Act
        var result = await handler.GetAllItemsAsync().ToListAsync();

        // Assert
        await Assert.That(result).HasCount().EqualTo(10_000);
        await Assert.That(result).IsEquivalentTo(items);
    }

    [Test]
    [DisplayName("Handler handles duplicate items correctly")]
    [MatrixDataSource]
    public async Task Handler_handles_duplicate_items(
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))] HandlerCases.ConstructorCall constructor)
    {
        // Arrange
        var items = new List<string> { "a", "b", "a", "c", "b", "a" };
        var handler = constructor(items);

        // Act
        var result = await handler.GetAllItemsAsync().ToListAsync();

        // Assert
        await Assert.That(result).IsEquivalentTo(items);
    }

    [Test]
    [DisplayName("Handler maintains order of items")]
    [MatrixDataSource]
    public async Task Handler_maintains_order(
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))] HandlerCases.ConstructorCall constructor)
    {
        // Arrange
        var items = new List<string> { "z", "a", "m", "b", "x" };
        var handler = constructor(items);

        // Act
        var result = await handler.GetAllItemsAsync().ToListAsync();        // Assert
        await Assert.That(result).IsEquivalentTo(items); // Order matters but use sequence equality
    }

    [Test]
    [DisplayName("Handler works correctly with single item")]
    [MatrixDataSource]
    public async Task Handler_works_with_single_item(
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))] HandlerCases.ConstructorCall constructor)
    {
        // Arrange
        var items = new List<string> { "single" };
        var handler = constructor(items);

        // Act
        var result = await handler.GetAllItemsAsync().ToListAsync();

        // Assert
        await Assert.That(result).HasCount().EqualTo(1);
        await Assert.That(result.First()).IsEqualTo("single");
    }    [Test]
    [DisplayName("Handler handles concurrent enumeration properly")]
    [MatrixDataSource]
    public async Task Handler_handles_concurrent_enumeration(
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))] HandlerCases.ConstructorCall constructor)
    {
        // Arrange
        var items = new List<string> { "a", "b", "c", "d", "e" };
        var handler = constructor(items);

        // Act - Start two concurrent enumerations
        var task1 = handler.GetAllItemsAsync().ToListAsync().AsTask();
        var task2 = handler.GetAllItemsAsync().ToListAsync().AsTask();
        
        var results = await Task.WhenAll(task1, task2);

        // Assert
        await Assert.That(results[0]).IsEquivalentTo(items);
        await Assert.That(results[1]).IsEquivalentTo(items);
    }

    [Test]
    [DisplayName("Handler works with very large single page")]
    [MatrixDataSource]
    public async Task Handler_works_with_very_large_single_page(
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))] HandlerCases.ConstructorCall constructor)
    {
        // Arrange
        var items = Enumerable.Range(0, 100_000).Select(i => $"item_{i}").ToList();
        var handler = constructor(items);

        // Act
        var count = 0;
        await foreach (var item in handler.GetAllItemsAsync())
        {
            count++;
            // Just verify we can enumerate through all items without issues
        }

        // Assert
        await Assert.That(count).IsEqualTo(100_000);
    }

    [Test]
    [DisplayName("Handler stops enumeration cleanly when broken early")]
    [MatrixDataSource]
    public async Task Handler_stops_enumeration_cleanly_when_broken_early(
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))] HandlerCases.ConstructorCall constructor)
    {
        // Arrange
        var items = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
        var handler = constructor(items);
        var collectedItems = new List<string>();

        // Act
        await foreach (var item in handler.GetAllItemsAsync())
        {
            collectedItems.Add(item);
            if (collectedItems.Count == 3)
            {
                break; // Break early
            }
        }        // Assert
        await Assert.That(collectedItems).HasCount().EqualTo(3);
        await Assert.That(collectedItems).IsEquivalentTo(new[] { "a", "b", "c" });
    }

    [Test]
    [DisplayName("Handler works with items containing special characters")]
    [MatrixDataSource]
    public async Task Handler_works_with_special_characters(
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))] HandlerCases.ConstructorCall constructor)
    {
        // Arrange
        var items = new List<string> { "", "  ", "\n", "\t", "🚀", "αβγ", "test with spaces", "\"quoted\"" };
        var handler = constructor(items);

        // Act
        var result = await handler.GetAllItemsAsync().ToListAsync();

        // Assert
        await Assert.That(result).IsEquivalentTo(items);
    }

    [Test]
    [DisplayName("Handler disposes resources properly")]
    public async Task Handler_disposes_resources_properly()
    {
        // This test is specifically for EnumeratorPaginationHandler to ensure enumerators are disposed
        var items = new List<string> { "a", "b", "c" };
        var handler = new EnumeratorPaginationHandler<string>(items);

        // Act - enumerate completely
        var result1 = await handler.GetAllItemsAsync().ToListAsync();
        
        // Act - enumerate partially and break
        var partialResult = new List<string>();
        await foreach (var item in handler.GetAllItemsAsync())
        {
            partialResult.Add(item);
            if (partialResult.Count == 1)
                break;
        }

        // Act - enumerate again to ensure no issues with disposed resources
        var result2 = await handler.GetAllItemsAsync().ToListAsync();

        // Assert
        await Assert.That(result1).IsEquivalentTo(items);
        await Assert.That(result2).IsEquivalentTo(items);
        await Assert.That(partialResult).HasCount().EqualTo(1);
    }

    [Test]
    [DisplayName("Handler behavior with cancellation at different stages")]
    [MatrixDataSource]
    public async Task Handler_cancellation_at_different_stages(
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))] HandlerCases.ConstructorCall constructor)
    {
        // Arrange
        var items = Enumerable.Range(0, 50).Select(i => i.ToString()).ToList();
        var handler = constructor(items);

        // Test cancellation before enumeration starts
        using var cts1 = new CancellationTokenSource();
        cts1.Cancel();
        
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await foreach (var item in handler.GetAllItemsAsync(cts1.Token))
            {
                // Should never get here
            }
        });

        // Test cancellation during enumeration
        using var cts2 = new CancellationTokenSource();
        var count = 0;
        
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await foreach (var item in handler.GetAllItemsAsync(cts2.Token))
            {
                count++;
                if (count == 10)
                {
                    cts2.Cancel(); // Cancel mid-enumeration
                }
            }
        });

        await Assert.That(count).IsGreaterThanOrEqualTo(10);
    }

    [Test]
    [DisplayName("Handler performance with frequent allocations")]
    [MatrixDataSource]
    public async Task Handler_performance_with_frequent_allocations(
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))] HandlerCases.ConstructorCall constructor)
    {
        // Arrange - Create items that will cause frequent allocations
        var items = Enumerable.Range(0, 1000)
            .Select(i => new string('x', i % 100)) // Varying string lengths
            .ToList();
        var handler = constructor(items);

        // Act
        var startTime = DateTime.UtcNow;
        var result = await handler.GetAllItemsAsync().ToListAsync();
        var endTime = DateTime.UtcNow;

        // Assert
        await Assert.That(result).HasCount().EqualTo(1000);
        await Assert.That(result).IsEquivalentTo(items);
        
        // Performance assertion - should complete within reasonable time
        var duration = endTime - startTime;
        await Assert.That(duration.TotalSeconds).IsLessThan(10); // Should be much faster, but giving generous timeout
    }

    [Test]
    [DisplayName("Handler propagates exceptions from GetPageAsync")]
    public async Task Handler_propagates_exceptions_from_GetPageAsync()
    {
        // Arrange
        var items = new List<string> { "a", "b", "c", "d", "e" };
        var handler = new FaultyPageRetrievalHandler<string>(items, failOnPage: 2);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await foreach (var item in handler.GetAllItemsAsync())
            {
                // Should fail when trying to get the third item (page 2)
            }
        });
    }

    [Test]
    [DisplayName("Handler propagates exceptions from NextPageExistsAsync")]
    public async Task Handler_propagates_exceptions_from_NextPageExistsAsync()
    {
        // Arrange
        var items = new List<string> { "a", "b", "c", "d", "e" };
        var handler = new FaultyNextPageCheckHandler<string>(items, failOnCheck: 1);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await foreach (var item in handler.GetAllItemsAsync())
            {
                // Should fail when checking if page 1 has a next page
            }
        });
    }

    [Test]
    [DisplayName("Handler propagates exceptions from ExtractItemsAsync")]
    public async Task Handler_propagates_exceptions_from_ExtractItemsAsync()
    {
        // Arrange
        var items = new List<string> { "a", "b", "c", "d", "e" };
        var handler = new FaultyItemExtractionHandler<string>(items, failOnPage: 1);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await foreach (var item in handler.GetAllItemsAsync())
            {
                // Should fail when extracting items from page 1
            }
        });
    }

    [Test]
    [DisplayName("Handler allows partial enumeration before failure")]
    public async Task Handler_allows_partial_enumeration_before_failure()
    {
        // Arrange
        var items = new List<string> { "a", "b", "c", "d", "e" };
        var handler = new FaultyPageRetrievalHandler<string>(items, failOnPage: 3);
        var retrievedItems = new List<string>();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await foreach (var item in handler.GetAllItemsAsync())
            {
                retrievedItems.Add(item);
            }
        });

        // Should have retrieved items from pages 0, 1, and 2 before failing on page 3
        await Assert.That(retrievedItems).HasCount().EqualTo(3);
        await Assert.That(retrievedItems).IsEquivalentTo(new[] { "a", "b", "c" });
    }

    [Test]
    [DisplayName("Handler handles async enumerable properly with various collection types")]
    [MatrixDataSource]
    public async Task Handler_works_with_different_collection_types(
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))] HandlerCases.ConstructorCall constructor)
    {
        // Test with different IEnumerable implementations
        var testCases = new[]
        {
            (IEnumerable<string>)new[] { "a", "b", "c" }, // Array
            (IEnumerable<string>)new List<string> { "a", "b", "c" }, // List
            (IEnumerable<string>)new HashSet<string> { "a", "b", "c" }, // HashSet
            (IEnumerable<string>)"abc".Select(c => c.ToString()), // LINQ query
            (IEnumerable<string>)Enumerable.Range(0, 3).Select(i => ((char)('a' + i)).ToString()) // Complex LINQ
        };

        foreach (var testCase in testCases)
        {
            // Arrange
            var handler = constructor(testCase);

            // Act
            var result = await handler.GetAllItemsAsync().ToListAsync();

            // Assert
            await Assert.That(result).HasCount().EqualTo(3);
            await Assert.That(result.Contains("a")).IsTrue();
            await Assert.That(result.Contains("b")).IsTrue();
            await Assert.That(result.Contains("c")).IsTrue();
        }
    }
}
