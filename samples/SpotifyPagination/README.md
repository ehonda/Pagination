# SpotifyPagination Sample

## Overview

This sample demonstrates how to use the `OffsetBased` library to interact with an offset-based paginated API. It showcases four approaches using the Spotify API as an example. For additional information about the Spotify API and pagination concepts, see the [References](#references) section.

1. 🏗️ **[Inheriting the abstract base class `OffsetBased.PaginationHandler`](#1-️-inheriting-from-paginationhandler)**  
   ✅ **Best for:** Quick implementation when you want to implement pagination logic within a single class

2. 🧩 **[Using `OffsetBased.Composite.PaginationHandler` by passing dependencies directly](#2--using-compositepaginationhandler-with-direct-dependencies)**  
   ✅ **Best for:** When you prefer composition over inheritance and want to decouple pagination logic into reusable components

3. 🔧 **[Using `OffsetBased.Composite.PaginationHandlerBuilder` with concrete composite implementations](#3--using-paginationhandlerbuilder-with-concrete-components)**  
   ✅ **Best for:** When you have existing concrete component implementations or prefer the builder pattern's readability

4. ⚡ **[Using `OffsetBased.Composite.PaginationHandlerBuilder` with lambda expressions](#4--using-paginationhandlerbuilder-with-lambda-expressions)**  
   ✅ **Best for:** Simple pagination scenarios where creating separate classes would be overkill, providing maximum flexibility with minimal code

Each approach illustrates a different level of flexibility and control when handling pagination.

## 1. 🏗️ Inheriting from `PaginationHandler`

The most direct way to use the `OffsetBased` library is to subclass the abstract base class `OffsetBased.PaginationHandler<TPaginationContext, TIndex, TItem>` and implement the required methods. The library also provides a two-parameter version `PaginationHandler<TPaginationContext, TItem>` that defaults the index type to `int`:

```csharp
// TPaginationContext: GetAlbumsResponse       (API response type)
// TIndex: int                                 (index type)
// TItem: Album                                (item type)
public class AlbumsPaginationHandler
    : PaginationHandler<GetAlbumsResponse, int, Album>
{
    // Spotify API client for fetching album data
    private readonly ArtistsClient _client;

    public AlbumsPaginationHandler(ArtistsClient client)
        => _client = client;

    // Retrieve the next page using the Spotify API client
    protected override async Task<GetAlbumsResponse> GetPageAsync(
        GetAlbumsResponse? context,
        CancellationToken cancellationToken = default)
    {
        const int limit = 10;
        var offset = context?.Offset + limit ?? 0;
        return await _client.GetAlbums(Ids.GraceJones, limit, offset);
    }

    // Extract the album items from the API response
    protected override IAsyncEnumerable<Album> ExtractItemsAsync(
        GetAlbumsResponse context,
        CancellationToken cancellationToken = default)
        => context.Items.ToAsyncEnumerable();

    // Extract the offset state (offset and total) for pagination
    protected override Task<OffsetState<int>> ExtractOffsetStateAsync(
        GetAlbumsResponse context,
        CancellationToken cancellationToken = default)
        => Task.FromResult(new OffsetState<int>(context.Offset, context.Total));
}
```

You can then use this handler to retrieve all items:

```csharp
// Instantiate the handler and retrieve albums
var handler = new AlbumsPaginationHandler(artistsClient);
// Fetch all albums using the pagination logic
var allAlbums = await handler
    .GetAllItemsAsync()
    .ToListAsync();
```

## 2. 🧩 Using `Composite.PaginationHandler` with Direct Dependencies

For scenarios where you prefer composition over inheritance, the library provides an `OffsetBased.Composite.PaginationHandler` that accepts dependencies directly. This approach separates concerns by delegating specific responsibilities to dedicated components.

The composite handler also has a two-parameter version `PaginationHandler<TPaginationContext, TItem>` that defaults the index type to `int`:

```csharp
// Create individual components for pagination logic
var pageRetriever = new PageRetriever(artistsClient);
var offsetStateExtractor = new OffsetStateExtractor();
var itemExtractor = new ItemExtractor();

// Compose them into a pagination handler
// TPaginationContext: GetAlbumsResponse, TItem: Album
var handler = new PaginationHandler<GetAlbumsResponse, Album>(
    pageRetriever,
    offsetStateExtractor,
    itemExtractor);

// Use the handler to retrieve albums
var allAlbums = await handler
    .GetAllItemsAsync()
    .ToListAsync();
```

This approach uses three separate components:

- **`PageRetriever`**: Handles API calls to fetch the next page
- **`OffsetStateExtractor`**: Extracts the offset and total count for pagination from the response
- **`ItemExtractor`**: Extracts the actual items from the API response

Each component implements a specific interface, making the pagination logic modular and testable.

Here are the concrete implementations of these components:

```csharp
public class PageRetriever : IPageRetriever<GetAlbumsResponse>
{
    private readonly ArtistsClient _client;

    public PageRetriever(ArtistsClient client)
        => _client = client;
    
    // Fetch the next page using the API client
    public Task<GetAlbumsResponse> GetAsync(GetAlbumsResponse? context,
        CancellationToken cancellationToken = default)
    {
        const int limit = 10;
        var offset = context?.Offset + limit ?? 0;
        return _client.GetAlbums(Ids.GraceJones, limit, offset);
    }
}

public class OffsetStateExtractor : IOffsetStateExtractor<GetAlbumsResponse>
{
    // Extract the offset and total count from the API response for pagination
    public Task<OffsetState<int>> ExtractOffsetStateAsync(GetAlbumsResponse context,
        CancellationToken cancellationToken = default) 
        => Task.FromResult(new OffsetState<int>(context.Offset, context.Total));
}

public class ItemExtractor : IItemExtractor<GetAlbumsResponse, Album>
{
    // Extract the actual album items from the API response
    public IAsyncEnumerable<Album> ExtractItemsAsync(GetAlbumsResponse context,
        CancellationToken cancellationToken = default)
        => context.Items.ToAsyncEnumerable();
}
```

## 3. 🔧 Using `PaginationHandlerBuilder` with Concrete Components

The `OffsetBased.Composite.PaginationHandlerBuilder` provides a fluent API for building pagination handlers. You can use it with concrete implementations of the component interfaces.

Like the other components in the library, the builder also has a two-parameter version `PaginationHandlerBuilder<TPaginationContext, TItem>` that defaults the index type to `int`:

```csharp
// Create the builder and configure it with concrete components
// TPaginationContext: GetAlbumsResponse, TItem: Album
var handler = new PaginationHandlerBuilder<GetAlbumsResponse, Album>()
    .WithPageRetriever(new PageRetriever(artistsClient))
    .WithOffsetStateExtractor(new OffsetStateExtractor())
    .WithItemExtractor(new ItemExtractor())
    .Build();

// Use the handler to retrieve albums
var allAlbums = await handler
    .GetAllItemsAsync()
    .ToListAsync();
```

This approach combines the flexibility of the builder pattern with the modularity of concrete components. The builder validates that all required components are provided and creates the pagination handler instance.

The concrete components (`PageRetriever`, `OffsetStateExtractor`, `ItemExtractor`) are the same as shown in Section 2, making this approach interchangeable with the direct composition approach.

## 4. ⚡ Using `PaginationHandlerBuilder` with Lambda Expressions

For maximum flexibility and minimal code, you can configure the builder using lambda expressions instead of concrete implementations. This approach eliminates the need to create separate classes for simple pagination logic.

Like other components in the library, the builder has a two-parameter version `PaginationHandlerBuilder<TPaginationContext, TItem>` that defaults the index type to `int` in the three-parameter version:

```csharp
// Create the builder and configure it with lambda expressions
// TPaginationContext: GetAlbumsResponse, TItem: Album
var handler = new PaginationHandlerBuilder<GetAlbumsResponse, Album>()
    .WithPageRetriever(async (context, cancellationToken) =>
    {
        const int limit = 10;
        var offset = context?.Offset + limit ?? 0;
        return await artistsClient.GetAlbums(Ids.GraceJones, limit, offset);
    })
    .WithOffsetStateExtractor(context => Task.FromResult(new OffsetState<int>(context.Offset, context.Total)))
    .WithItemExtractor(context => context.Items.ToAsyncEnumerable())
    .Build();

// Use the handler to retrieve albums
var allAlbums = await handler
    .GetAllItemsAsync()
    .ToListAsync();
```

This approach provides the same functionality as the previous examples but with inline implementations, making it ideal for simple pagination scenarios where creating separate classes would be overkill. The builder automatically wraps your lambda expressions in the appropriate interface implementations.

## References

### Spotify API Documentation

- **[Spotify Web API Pagination Guide](https://developer.spotify.com/documentation/web-api/concepts/api-calls#pagination)** - Official documentation explaining offset-based pagination in the Spotify API
- **[Get Artist's Albums API Reference](https://developer.spotify.com/documentation/web-api/reference/get-an-artists-albums)** - API reference for the endpoint used in this sample

### Related Resources

- **[Spotify Web API Getting Started](https://developer.spotify.com/documentation/web-api/)** - General introduction to the Spotify Web API
- **[Authentication Guide](https://developer.spotify.com/documentation/web-api/concepts/authorization)** - How to authenticate with the Spotify API (required for API access)
