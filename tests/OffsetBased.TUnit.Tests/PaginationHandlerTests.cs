using System.Runtime.CompilerServices;
using Core;
using OffsetBased.Composite;

namespace OffsetBased.TUnit.Tests;

public record Page<TItem>(IReadOnlyList<TItem> Items, int Offset, int Total);

public class PaginationHandlerTests
{
    private static IPaginationHandler<TItem> CreateWorkingHandler<TItem>(IReadOnlyList<TItem> items, int pageSize)
    {
        return new PaginationHandlerBuilder<Page<TItem>, int, TItem>()
            .WithPageRetriever((context, _) =>
            {
                var offset = context is null ? 0 : context.Offset + context.Items.Count;
                var pageItems = items.Skip(offset).Take(pageSize).ToList();
                return Task.FromResult(new Page<TItem>(pageItems, offset, items.Count));
            })
            .WithOffsetStateExtractor(page => new OffsetState<int>(page.Offset, page.Total))
            .WithItemExtractor(page => page.Items)
            .Build();
    }

    private static IPaginationHandler<TItem> CreateFaultyPageRetrievalHandler<TItem>(IReadOnlyList<TItem> items,
        int pageSize, int failOnPage)
    {
        var currentPage = 0;
        return new PaginationHandlerBuilder<Page<TItem>, int, TItem>()
            .WithPageRetriever((context, _) =>
            {
                if (currentPage++ == failOnPage)
                {
                    throw new InvalidOperationException("Simulated failure");
                }

                var offset = context is null ? 0 : context.Offset + context.Items.Count;
                var pageItems = items.Skip(offset).Take(pageSize).ToList();
                return Task.FromResult(new Page<TItem>(pageItems, offset, items.Count));
            })
            .WithOffsetStateExtractor(page => new OffsetState<int>(page.Offset, page.Total))
            .WithItemExtractor(page => page.Items)
            .Build();
    }

    private static IPaginationHandler<TItem> CreateFaultyOffsetStateExtractionHandler<TItem>(IReadOnlyList<TItem> items,
        int pageSize, int failOnPage)
    {
        var currentPage = 0;
        return new PaginationHandlerBuilder<Page<TItem>, int, TItem>()
            .WithPageRetriever((context, _) =>
            {
                var offset = context is null ? 0 : context.Offset + context.Items.Count;
                var pageItems = items.Skip(offset).Take(pageSize).ToList();
                return Task.FromResult(new Page<TItem>(pageItems, offset, items.Count));
            })
            .WithOffsetStateExtractor(page =>
            {
                if (currentPage++ == failOnPage)
                {
                    throw new InvalidOperationException("Simulated failure");
                }

                return new OffsetState<int>(page.Offset, page.Total);
            })
            .WithItemExtractor(page => page.Items)
            .Build();
    }

    private static IPaginationHandler<TItem> CreateFaultyItemExtractionHandler<TItem>(IReadOnlyList<TItem> items,
        int pageSize, int failOnPage)
    {
        var currentPage = 0;
        return new PaginationHandlerBuilder<Page<TItem>, int, TItem>()
            .WithPageRetriever((context, _) =>
            {
                var offset = context is null ? 0 : context.Offset + context.Items.Count;
                var pageItems = items.Skip(offset).Take(pageSize).ToList();
                return Task.FromResult(new Page<TItem>(pageItems, offset, items.Count));
            })
            .WithOffsetStateExtractor(page => new OffsetState<int>(page.Offset, page.Total))
            .WithItemExtractor(page =>
            {
                if (currentPage++ == failOnPage)
                {
                    throw new InvalidOperationException("Simulated failure");
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

        public static IEnumerable<List<string>> All() => typeof(ListCases).GetMethods()
            .Where(m => m.ReturnType == typeof(List<string>)).Select(m => (List<string>) m.Invoke(null, null)!);
    }

    public class HandlerCases
    {
        public record ConstructorCall(
            Func<IReadOnlyList<string>, IPaginationHandler<string>> Constructor,
            [CallerArgumentExpression(nameof(Constructor))]
            string Name = "");

        public static ConstructorCall Working() => new(items => CreateWorkingHandler(items, 2));

        public static ConstructorCall FaultyPageRetrieval() =>
            new(items => CreateFaultyPageRetrievalHandler(items, 2, 1));

        public static ConstructorCall FaultyOffsetStateExtraction() =>
            new(items => CreateFaultyOffsetStateExtractionHandler(items, 2, 1));

        public static ConstructorCall FaultyItemExtraction() =>
            new(items => CreateFaultyItemExtractionHandler(items, 2, 1));

        public static IEnumerable<ConstructorCall> All() => new[] { Working() };
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
        var handler = constructor.Constructor(items);
        var results = await handler.GetAllItemsAsync().ToListAsync();
        await Assert.That(results).IsEquivalentTo(items);
    }

    [Test]
    [DisplayName("Handler respects cancellation token")]
    [MatrixDataSource]
    public async Task Handler_respects_cancellation_token(
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))]
        HandlerCases.ConstructorCall constructor)
    {
        var items = new List<string> { "1", "2", "3", "4", "5" };
        var handler = constructor.Constructor(items);
        var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
        {
            await handler.GetAllItemsAsync(cts.Token).ToListAsync();
        });
    }

    [Test]
    [DisplayName("Handler works with null items in collection")]
    [MatrixDataSource]
    public async Task Handler_works_with_null_items(
        [MatrixMethod<HandlerCases>(nameof(HandlerCases.All))]
        HandlerCases.ConstructorCall constructor)
    {
        var items = new List<string?> { "1", null, "3" };
        var handler = constructor.Constructor(items!);
        var results = await handler.GetAllItemsAsync().ToListAsync();
        await Assert.That(results).IsEquivalentTo(items);
    }

    [Test]
    [DisplayName("Faulty page retrieval is handled")]
    public async Task Faulty_page_retrieval_is_handled()
    {
        var items = new List<string> { "1", "2", "3", "4", "5" };
        var handler = CreateFaultyPageRetrievalHandler(items, 2, 1);

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await handler.GetAllItemsAsync().ToListAsync();
        });
    }

    [Test]
    [DisplayName("Faulty offset state extraction is handled")]
    public async Task Faulty_offset_state_extraction_is_handled()
    {
        var items = new List<string> { "1", "2", "3", "4", "5" };
        var handler = CreateFaultyOffsetStateExtractionHandler(items, 2, 1);

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await handler.GetAllItemsAsync().ToListAsync();
        });
    }

    [Test]
    [DisplayName("Faulty item extraction is handled")]
    public async Task Faulty_item_extraction_is_handled()
    {
        var items = new List<string> { "1", "2", "3", "4", "5" };
        var handler = CreateFaultyItemExtractionHandler(items, 2, 1);

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await handler.GetAllItemsAsync().ToListAsync();
        });
    }
}
