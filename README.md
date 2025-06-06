# EHonda.Pagination

🚧 WIP 🚧

## Description

`EHonda.Pagination` is a .NET library that provides a flexible and extensible way to handle pagination. It offers core interfaces and building blocks, along with implementations for cursor-based, offset-based, and sequential pagination. This repository also includes sample projects in the `samples` directory to demonstrate the library's usage in various scenarios.

## Features and Examples

This library provides several ways to handle pagination, catering to different API designs. Below are some examples of how to use the core pagination handlers, focusing on the fluent builder syntax for a more declarative setup. For more detailed and runnable examples, please refer to the projects in the `samples` directory.

### Offset-Based Pagination

Offset-based pagination uses an index or offset to retrieve specific pages of data. This is common in many web APIs.

**Example using the fluent builder:**

```csharp
// Assumes MyPage, MyItem types, and int for the offset index.
// MyPage is the type returned by your API client, containing items and pagination details.
// e.g., public class MyPage { public List<MyItem> Items; public int CurrentOffset; public int PageSize; public int TotalItems; /* ... */ }

var apiClient = new YourApiClient(); // Your actual API client
const int initialOffset = 0;
const int pageSize = 20;

// Or just the shortcut `OffsetBased.Composite.PaginationHandlerBuilder<MyPage, MyItem>`
var handler = new OffsetBased.Composite.PaginationHandlerBuilder<MyPage, int, MyItem>()
    .WithPageRetriever(async (MyPage? prevPage, CancellationToken cancellationToken) =>
    {
        int currentOffset;
        if (prevPage == null)
        {
            currentOffset = initialOffset; // First page
        }
        else
        {
            // Calculate offset for the next page based on the previous one
            currentOffset = prevPage.CurrentOffset + prevPage.PageSize;
        }
        return await apiClient.GetPageByOffsetAsync(currentOffset, pageSize, cancellationToken);
    })
    .WithOffsetStateExtractor((MyPage pageContext, CancellationToken cancellationToken) =>
    {
        // Extract pagination state from the fetched page
        return new OffsetState<int>(
            pageContext.CurrentOffset,
            pageContext.PageSize,
            pageContext.TotalItems);
    })
    .WithItemExtractor((MyPage pageContext, CancellationToken cancellationToken) =>
    {
        // Extract items from the fetched page
        return pageContext.Items;
    })
    .Build();

await foreach (var item in handler.GetAllItemsAsync())
{
    // Process each item
}
```

The library provides `OffsetState<TIndex>` (see `src/OffsetBased/OffsetState.cs`) to manage the current offset. The base handler is in `src/OffsetBased/PaginationHandler.cs`. For concrete examples, see the `samples/SpotifyPagination` project.

### Cursor-Based Pagination

Cursor-based pagination uses a cursor (often an item's ID or a timestamp) to mark the position from which to fetch the next set of items. This is efficient for large datasets.

**Example using the fluent builder (lambda-based components):**

```csharp
// Assumes MyPageContext, TCursor, and MyItem types.
// MyPageContext is the type returned by your API client for a page of data.
// It should contain:
//  - A list of items (e.g., public List<MyItem> Items { get; set; })
//  - The cursor for the next page (e.g., public TCursor? NextPageCursor { get; set; })

var apiClient = new YourApiClient(); // Your actual API client

// If TCursor is string, you can use the shortcut: 
// var handler = new CursorBased.Composite.PaginationHandlerBuilder<MyPageContext, MyItem>()
var handler = new CursorBased.Composite.PaginationHandlerBuilder<MyPageContext, TCursor, MyItem>()
    .WithPageRetriever(async (MyPageContext? prevPageContext, CancellationToken cancellationToken) =>
    {
        if (prevPageContext == null)
        {
            // First call: fetch the initial page (e.g., API might not require a cursor or use a default one).
            return await apiClient.FetchPageAsync(null, cancellationToken); 
        }
        // Subsequent call: fetch the next page using the cursor from the *previous* page's context.
        // prevPageContext.NextPageCursor would have been extracted by WithCursorExtractor in the previous step.
        return await apiClient.FetchPageAsync(prevPageContext.NextPageCursor, cancellationToken);
    })
    // Or async: (MyPageContext currentPageContext, CancellationToken ct) => Task.FromResult(currentPageContext.NextPageCursor)
    .WithCursorExtractor((MyPageContext currentPageContext) => 
    {
        // Extract the cursor that points to the *next* page, from the *current* page's context.
        return currentPageContext.NextPageCursor;
    })
    // Or async: (MyPageContext currentPageContext, CancellationToken ct) => currentPageContext.ItemsAsync
    .WithItemExtractor((MyPageContext currentPageContext) => 
    {
        // Extract the list of items from the *current* page's context.
        return currentPageContext.Items;
    })
    .Build();

await foreach (var item in handler.GetAllItemsAsync())
{
    // Process each item
}
```

The base handler is in `src/CursorBased/PaginationHandler.cs`. For concrete examples, see the `samples/TwitchPagination` (demonstrates both lambda and class-based component setup) and `samples/SpotifyPagination` projects.

### Extensibility

All pagination handlers can be created by:

1. Inheriting from the base classes (e.g., `Sequential.PaginationHandler<TPageContext, TItem>`).
2. Using the composite `PaginationHandlerBuilder` for a more declarative setup (e.g., `Sequential.Composite.PaginationHandlerBuilder<TPageContext, TItem>`).

This allows for flexibility in how you define page retrieval, item extraction, and next-page-existence logic.
