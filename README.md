# EHonda.Pagination

## Description

`EHonda.Pagination` is a .NET library that provides a flexible and extensible way to handle pagination. It offers core interfaces and building blocks, along with implementations for cursor-based, offset-based, and sequential pagination. This repository also includes sample projects in the `samples` directory to demonstrate the library's usage in various scenarios.

## Quickstart

This example demonstrates how to use the cursor-based pagination handler to fetch all top games from the Twitch API via a `GamesClient`.

First, let's consider a `GamesClient` that can only fetch a single page of games at a time. The API returns a `GetTopGamesResponse` object containing the game data and a pagination cursor.

```csharp
// A client that fetches pages of data from the Twitch game API
public class GamesClient
{
    // Fetches a single page of top games
    public async Task<GetTopGamesResponse> GetTopGamesAsync(int first = 20, string? cursor = null)
    {
        // In a real application, this would make an HTTP request to the Twitch API.
        // For details, see the TwitchPagination sample.
    }
}

// Represents the API response for a page of games
public record GetTopGamesResponse(IReadOnlyList<Game> Data, Pagination Pagination);

public record Pagination(string? Cursor);
public record Game(string Id, string Name);
```

Instead of manually calling `GetTopGamesAsync` in a loop, we can use the `PaginationHandlerBuilder` to easily add a method that fetches all pages automatically.

```csharp
public class GamesClient
{
    // GetTopGamesAsync method as defined above
    // ...
    
    public IAsyncEnumerable<Game> GetAllTopGames()
    {
        var paginationHandler = new CursorBased.Composite.PaginationHandlerBuilder<GetTopGamesResponse, Game>()
            .WithPageRetriever((prevPage, _) => GetTopGamesAsync(100, prevPage?.Pagination.Cursor))
            .WithCursorExtractor(page => page.Pagination.Cursor)
            .WithItemExtractor(page => page.Data)
            .Build();

        return paginationHandler.GetAllItemsAsync();
    }
}
```

With this new method, you can now create a `GamesClient` instance and iterate through all games from all pages with a simple loop.

```csharp
var gamesClient = new GamesClient();
await foreach (var game in gamesClient.GetAllTopGames())
{
    Console.WriteLine(game.Name);
}
```

This approach simplifies pagination by abstracting the page-by-page fetching logic, allowing you to work with a single asynchronous stream of items. For a complete, runnable implementation, please see the **[TwitchPagination sample](samples/TwitchPagination/README.md)**.

## Sample Projects

The following sample projects demonstrate the `EHonda.Pagination` library in different contexts and scenarios:

### API-Based Samples

Both API-based sample projects follow the same comprehensive tutorial structure, covering four different implementation approaches with complete code examples and guidance on when to use each pattern: inheritance from base classes, direct composition with concrete components, builder pattern with concrete implementations, and builder pattern with lambda expressions.

- **[TwitchPagination](samples/TwitchPagination/README.md)** - Demonstrates cursor-based pagination using the Twitch API

- **[SpotifyPagination](samples/SpotifyPagination/README.md)** - Shows offset-based pagination using the Spotify API

### Database/ORM Sample

The EFCorePagination sample showcases the library's flexibility beyond traditional API pagination scenarios, demonstrating how the same pagination patterns can be effectively applied to database queries and ORM operations:

- **[EFCorePagination](samples/EFCorePagination/README.md)** - Demonstrates the library's versatility by applying both cursor-based and offset-based pagination to Entity Framework Core queries, showing how the pagination abstractions work seamlessly with database operations

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

The library provides `OffsetState<TIndex>` (see `src/OffsetBased/OffsetState.cs`) to manage the current offset. The base handler is in `src/OffsetBased/PaginationHandler.cs`. For concrete examples, see the **[SpotifyPagination sample](samples/SpotifyPagination/README.md)** project.

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

The base handler is in `src/CursorBased/PaginationHandler.cs`. For concrete examples, see the **[TwitchPagination sample](samples/TwitchPagination/README.md)** (demonstrates both lambda and class-based component setup) and **[SpotifyPagination sample](samples/SpotifyPagination/README.md)** projects.

### Extensibility

All pagination handlers can be created by:

1. Inheriting from the base classes (e.g., `Sequential.PaginationHandler<TPageContext, TItem>`).
2. Using the composite `PaginationHandlerBuilder` for a more declarative setup (e.g., `Sequential.Composite.PaginationHandlerBuilder<TPageContext, TItem>`).

This allows for flexibility in how you define page retrieval, item extraction, and next-page-existence logic.
