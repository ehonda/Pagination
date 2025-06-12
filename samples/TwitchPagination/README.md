# TwitchPagination Sample

## Overview

This sample demonstrates how to use the `CursorBased` library to interact with a cursor-based paginated API. It showcases four approaches using the Twitch API as an example. For additional information about the Twitch API and pagination concepts, see the [References](#references) section.

1. 🏗️ **[Inheriting the abstract base class `CursorBased.PaginationHandler`](#1-️-inheriting-from-paginationhandler)**  
   ✅ **Best for:** Quick implementation when you want to implement pagination logic within a single class

2. 🧩 **[Using `CursorBased.Composite.PaginationHandler` by passing dependencies directly](#2--using-compositepaginationhandler-with-direct-dependencies)**  
   ✅ **Best for:** When you prefer composition over inheritance and want to decouple pagination logic into reusable components

3. 🔧 **[Using `CursorBased.Composite.PaginationHandlerBuilder` with concrete composite implementations](#3--using-paginationhandlerbuilder-with-concrete-components)**  
   ✅ **Best for:** When you have existing concrete component implementations or prefer the builder pattern's readability

4. ⚡ **[Using `CursorBased.Composite.PaginationHandlerBuilder` with lambda expressions](#4--using-paginationhandlerbuilder-with-lambda-expressions)**  
   ✅ **Best for:** Simple pagination scenarios where creating separate classes would be overkill, providing maximum flexibility with minimal code

Each approach illustrates a different level of flexibility and control when handling pagination.

## 1. 🏗️ Inheriting from `PaginationHandler`

The most direct way to use the `CursorBased` library is to subclass the abstract base class `CursorBased.PaginationHandler<TPaginationContext, TCursor, TItem>` and implement the required methods. The library also provides a two-parameter version `PaginationHandler<TPaginationContext, TItem>` that defaults the cursor type to `string`:

```csharp
// TPaginationContext: GetTopGamesResponse       (API response type)
// TCursor: string                              (cursor type)
// TItem: Game                                  (item type)
public class TopGamesPaginationHandler
    : PaginationHandler<GetTopGamesResponse, string, Game>
{
    // Twitch API client for fetching game data
    private readonly GamesClient _client;

    public TopGamesPaginationHandler(GamesClient client)
        => _client = client;

    // Retrieve the next page using the Twitch API client
    protected override async Task<GetTopGamesResponse> GetPageAsync(
        GetTopGamesResponse? context,
        CancellationToken cancellationToken = default)
        => await _client.GetTopGames(100, context?.Pagination.Cursor);

    // Extract the game items from the API response
    protected override IAsyncEnumerable<Game> ExtractItemsAsync(
        GetTopGamesResponse context,
        CancellationToken cancellationToken = default)
        => context.Data.ToAsyncEnumerable();

    // Extract the cursor for the next page
    protected override Task<string?> ExtractCursorAsync(GetTopGamesResponse context)
        => Task.FromResult(context.Pagination.Cursor);
}
```

You can then use this handler to retrieve all items:

```csharp
// Instantiate the handler and retrieve games
var handler = new TopGamesPaginationHandler(gamesClient);
// Fetch up to 100 games using the pagination logic
var allGames = await handler
    .GetAllItemsAsync()
    .Take(100)
    .ToListAsync();
```

## 2. 🧩 Using `Composite.PaginationHandler` with Direct Dependencies

For scenarios where you prefer composition over inheritance, the library provides a `CursorBased.Composite.PaginationHandler` that accepts dependencies directly. This approach separates concerns by delegating specific responsibilities to dedicated components.

The composite handler also has a two-parameter version `PaginationHandler<TPaginationContext, TItem>` that defaults the cursor type to `string`:

```csharp
// Create individual components for pagination logic
var pageRetriever = new PageRetriever(gamesClient);
var cursorExtractor = new CursorExtractor();
var itemExtractor = new ItemExtractor();

// Compose them into a pagination handler
// TPaginationContext: GetTopGamesResponse, TItem: Game
var handler = new PaginationHandler<GetTopGamesResponse, Game>(
    pageRetriever,
    cursorExtractor,
    itemExtractor);

// Use the handler to retrieve games
var allGames = await handler
    .GetAllItemsAsync()
    .Take(100)
    .ToListAsync();
```

This approach uses three separate components:

- **`PageRetriever`**: Handles API calls to fetch the next page
- **`CursorExtractor`**: Extracts the cursor for pagination from the response
- **`ItemExtractor`**: Extracts the actual items from the API response

Each component implements a specific interface, making the pagination logic modular and testable.

Here are the concrete implementations of these components:

```csharp
public class PageRetriever : IPageRetriever<GetTopGamesResponse>
{
    private readonly GamesClient _client;

    public PageRetriever(GamesClient client)
        => _client = client;
    
    // Fetch the next page using the API client
    public Task<GetTopGamesResponse> GetAsync(GetTopGamesResponse? context,
        CancellationToken cancellationToken = default)
        => _client.GetTopGames(100, context?.Pagination.Cursor);
}

public class CursorExtractor : ICursorExtractor<GetTopGamesResponse>
{
    // Extract the cursor from the API response for pagination
    public Task<string?> ExtractCursorAsync(GetTopGamesResponse context)
        => Task.FromResult(context.Pagination.Cursor);
}

public class ItemExtractor : IItemExtractor<GetTopGamesResponse, Game>
{
    // Extract the actual game items from the API response
    public IAsyncEnumerable<Game> ExtractItemsAsync(GetTopGamesResponse context,
        CancellationToken cancellationToken = default)
        => context.Data.ToAsyncEnumerable();
}
```

## 3. 🔧 Using `PaginationHandlerBuilder` with Concrete Components

The `CursorBased.Composite.PaginationHandlerBuilder` provides a fluent API for building pagination handlers. You can use it with concrete implementations of the component interfaces.

Like the other components in the library, the builder also has a two-parameter version `PaginationHandlerBuilder<TPaginationContext, TItem>` that defaults the cursor type to `string`:

```csharp
// Create the builder and configure it with concrete components
// TPaginationContext: GetTopGamesResponse, TItem: Game
var handler = new PaginationHandlerBuilder<GetTopGamesResponse, Game>()
    .WithPageRetriever(new PageRetriever(gamesClient))
    .WithCursorExtractor(new CursorExtractor())
    .WithItemExtractor(new ItemExtractor())
    .Build();

// Use the handler to retrieve games
var allGames = await handler
    .GetAllItemsAsync()
    .Take(100)
    .ToListAsync();
```

This approach combines the flexibility of the builder pattern with the modularity of concrete components. The builder validates that all required components are provided and creates the pagination handler instance.

The concrete components (`PageRetriever`, `CursorExtractor`, `ItemExtractor`) are the same as shown in Section 2, making this approach interchangeable with the direct composition approach.

## 4. ⚡ Using `PaginationHandlerBuilder` with Lambda Expressions

For maximum flexibility and minimal code, you can configure the builder using lambda expressions instead of concrete implementations. This approach eliminates the need to create separate classes for simple pagination logic.

Like other components in the library, the builder has a two-parameter version `PaginationHandlerBuilder<TPaginationContext, TItem>` that defaults the cursor type to `string` in the three-parameter version:

```csharp
// Create the builder and configure it with lambda expressions
// TPaginationContext: GetTopGamesResponse, TItem: Game
var handler = new PaginationHandlerBuilder<GetTopGamesResponse, Game>()
    .WithPageRetriever(
        async (context, cancellationToken) => await gamesClient.GetTopGames(100, context?.Pagination.Cursor))
    .WithCursorExtractor(context => Task.FromResult(context.Pagination.Cursor))
    .WithItemExtractor(context => context.Data.ToAsyncEnumerable())
    .Build();

// Use the handler to retrieve games
var allGames = await handler
    .GetAllItemsAsync()
    .Take(100)
    .ToListAsync();
```

This approach provides the same functionality as the previous examples but with inline implementations, making it ideal for simple pagination scenarios where creating separate classes would be overkill. The builder automatically wraps your lambda expressions in the appropriate interface implementations.

## References

### Twitch API Documentation

- **[Twitch API Pagination Guide](https://dev.twitch.tv/docs/api/guide#pagination)** - Official documentation explaining cursor-based pagination in the Twitch API
- **[Get Top Games API Reference](https://dev.twitch.tv/docs/api/reference#get-top-games)** - API reference for the endpoint used in this sample

### Related Resources

- **[Twitch API Getting Started](https://dev.twitch.tv/docs/api/)** - General introduction to the Twitch API
- **[Authentication Guide](https://dev.twitch.tv/docs/authentication/)** - How to authenticate with the Twitch API (required for API access)
