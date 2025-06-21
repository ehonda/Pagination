using EHonda.Pagination.Core;
using EHonda.Pagination.CursorBased.Composite;

namespace CursorBased.TUnit.Tests;

public record Page<TItem>(IReadOnlyList<TItem> Items, string? Cursor);

public class PaginationHandlerTests
{
    private static IPaginationHandler<TItem> CreateWorkingHandler<TItem>(IReadOnlyList<TItem> items, int pageSize)
    {
        return new PaginationHandlerBuilder<Page<TItem>, string, TItem>()
            .WithPageRetriever((context, _) =>
            {
                var cursor = context?.Cursor;

                var number = cursor is null ? 0 : int.Parse(cursor);

                var page = new Page<TItem>(items.Skip(number).Take(pageSize).ToList(), (number + pageSize).ToString());

                return Task.FromResult(page);
            })
            .WithCursorExtractor(page =>
            {
                if (page.Items.Count == 0)
                {
                    return null;
                }

                return page.Cursor;
            })
            .WithItemExtractor(page => page.Items)
            .Build();
    }

    private static IPaginationHandler<TItem> CreateFaultyPageRetrievalHandler<TItem>(IReadOnlyList<TItem> items,
        int pageSize, int failOnPage)
    {
        var currentPage = 0;
        return new PaginationHandlerBuilder<Page<TItem>, string, TItem>()
            .WithPageRetriever((context, _) =>
            {
                if (currentPage++ == failOnPage)
                {
                    throw new Exception("Faulty page retrieval");
                }

                var cursor = context?.Cursor;

                var number = cursor is null ? 0 : int.Parse(cursor);

                var page = new Page<TItem>(items.Skip(number).Take(pageSize).ToList(), (number + pageSize).ToString());

                return Task.FromResult(page);
            })
            .WithCursorExtractor(page =>
            {
                if (page.Items.Count == 0)
                {
                    return null;
                }

                return page.Cursor;
            })
            .WithItemExtractor(page => page.Items)
            .Build();
    }

    private static IPaginationHandler<TItem> CreateFaultyCursorExtractionHandler<TItem>(IReadOnlyList<TItem> items,
        int pageSize, int failOnPage)
    {
        var currentPage = 0;
        return new PaginationHandlerBuilder<Page<TItem>, string, TItem>()
            .WithPageRetriever((context, _) =>
            {
                var cursor = context?.Cursor;

                var number = cursor is null ? 0 : int.Parse(cursor);

                var page = new Page<TItem>(items.Skip(number).Take(pageSize).ToList(), (number + pageSize).ToString());

                return Task.FromResult(page);
            })
            .WithCursorExtractor(page =>
            {
                if (currentPage++ == failOnPage)
                {
                    throw new Exception("Faulty cursor extraction");
                }

                if (page.Items.Count == 0)
                {
                    return null;
                }

                return page.Cursor;
            })
            .WithItemExtractor(page => page.Items)
            .Build();
    }

    private static IPaginationHandler<TItem> CreateFaultyItemExtractionHandler<TItem>(IReadOnlyList<TItem> items,
        int pageSize, int failOnPage)
    {
        var currentPage = 0;
        return new PaginationHandlerBuilder<Page<TItem>, string, TItem>()
            .WithPageRetriever((context, _) =>
            {
                var cursor = context?.Cursor;

                var number = cursor is null ? 0 : int.Parse(cursor);

                var page = new Page<TItem>(items.Skip(number).Take(pageSize).ToList(), (number + pageSize).ToString());

                return Task.FromResult(page);
            })
            .WithCursorExtractor(page =>
            {
                if (page.Items.Count == 0)
                {
                    return null;
                }

                return page.Cursor;
            })
            .WithItemExtractor(page =>
            {
                if (currentPage++ == failOnPage)
                {
                    throw new Exception("Faulty item extraction");
                }

                return page.Items;
            })
            .Build();
    }

    public class ListCases
    {
        public static List<string> Empty() => [];
        public static List<string> One() => ["1"];
        public static List<string> Two() => ["1", "2"];
        public static List<string> Three() => ["1", "2", "3"];
        public static List<string> Four() => ["1", "2", "3", "4"];
        public static List<string> Five() => ["1", "2", "3", "4", "5"];
        public static List<string> Six() => ["1", "2", "3", "4", "5", "6"];

        public static IEnumerable<List<string>> All()
        {
            yield return Empty();
            yield return One();
            yield return Two();
            yield return Three();
            yield return Four();
            yield return Five();
            yield return Six();
        }
    }

    public class HandlerCases
    {
        public delegate IPaginationHandler<string> ConstructorCall(IReadOnlyList<string> items, int pageSize);

        public static ConstructorCall Working() => CreateWorkingHandler;

        public static ConstructorCall FaultyPageRetrieval() =>
            (items, pageSize) => CreateFaultyPageRetrievalHandler(items, pageSize, 1);

        public static ConstructorCall FaultyCursorExtraction() =>
            (items, pageSize) => CreateFaultyCursorExtractionHandler(items, pageSize, 1);

        public static ConstructorCall FaultyItemExtraction() =>
            (items, pageSize) => CreateFaultyItemExtractionHandler(items, pageSize, 1);

        public static IEnumerable<ConstructorCall> All()
        {
            yield return Working();
        }
    }

    [Test]
    [DisplayName("The PaginationHandler works")]
    [MatrixDataSource]
    public async Task Handler_works(
        [MatrixMethod<ListCases>(nameof(ListCases.All))]
        List<string> items,
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))]
        HandlerCases.ConstructorCall constructor)
    {
        // Arrange
        var pageSize = 2;
        var handler = constructor(items, pageSize);

        // Act
        var result = await handler.GetAllItemsAsync().ToListAsync();

        // Assert
        await Assert.That(result).IsEquivalentTo(items);
    }

    [Test]
    [DisplayName("Handler respects cancellation token")]
    public async Task Handler_respects_cancellation_token()
    {
        // Arrange
        var items = new[] { "1", "2", "3", "4", "5", "6" };
        var pageSize = 2;
        var handler = CreateWorkingHandler(items, pageSize);
        var cancellationTokenSource = new CancellationTokenSource();

        // Act
        var result = new List<string>();
        await foreach (var item in handler.GetAllItemsAsync(cancellationTokenSource.Token))
        {
            result.Add(item);
            if (result.Count == 3)
            {
                await cancellationTokenSource.CancelAsync();
            }
        }

        // Assert
        await Assert.That(result).IsEquivalentTo(items.Take(3));
    }

    [Test]
    [DisplayName("Handler works with null items in collection")]
    public async Task Handler_works_with_null_items()
    {
        // Arrange
        var items = new[] { "1", null, "3", "4", null, "6" };
        var pageSize = 2;
        var handler = CreateWorkingHandler(items, pageSize);

        // Act
        var result = await handler.GetAllItemsAsync().ToListAsync();

        // Assert
        await Assert.That(result).IsEquivalentTo(items);
    }

    [Test]
    [DisplayName("Faulty page retrieval is handled")]
    public async Task Faulty_page_retrieval_is_handled()
    {
        // Arrange
        var items = new[] { "1", "2", "3", "4", "5", "6" };
        var pageSize = 2;
        var handler = CreateFaultyPageRetrievalHandler(items, pageSize, 1);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(async () => await handler.GetAllItemsAsync().ToListAsync());
        await Assert.That(exception!.Message).IsEqualTo("Faulty page retrieval");
    }

    [Test]
    [DisplayName("Faulty cursor extraction is handled")]
    public async Task Faulty_cursor_extraction_is_handled()
    {
        // Arrange
        var items = new[] { "1", "2", "3", "4", "5", "6" };
        var pageSize = 2;
        var handler = CreateFaultyCursorExtractionHandler(items, pageSize, 1);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(async () => await handler.GetAllItemsAsync().ToListAsync());
        await Assert.That(exception!.Message).IsEqualTo("Faulty cursor extraction");
    }

    [Test]
    [DisplayName("Faulty item extraction is handled")]
    public async Task Faulty_item_extraction_is_handled()
    {
        // Arrange
        var items = new[] { "1", "2", "3", "4", "5", "6" };
        var pageSize = 2;
        var handler = CreateFaultyItemExtractionHandler(items, pageSize, 1);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(async () => await handler.GetAllItemsAsync().ToListAsync());
        await Assert.That(exception!.Message).IsEqualTo("Faulty item extraction");
    }
}
