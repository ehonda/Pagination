using ArrayPaginationHandler;
using Sequential.Composite;

List<int> pages = [1, 2, 3, 4, 5];

var firstPage = pages.GetEnumerator();
firstPage.MoveNext();

var handler = new CompositePaginationHandler<IEnumerator<int>, IEnumerator<int>, int>(
    new PageRetriever(firstPage),
    new PaginationContextExtractor(),
    new NextPageChecker(),
    new ItemExtractor());

var items = await handler.GetAllItemsAsync().ToListAsync();

foreach (var item in items)
{
    Console.WriteLine(item);
}
