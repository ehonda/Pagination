# EFCore Pagination Example

This sample demonstrates how to implement both cursor-based and offset-based pagination with Entity Framework Core using the EHonda.Pagination library's fluent builder pattern.

## Overview

The example uses a simple `Movie` entity with an SQLite database to showcase:

- **Cursor-based pagination**: Efficient for large datasets, uses the last item's ID as a cursor
- **Offset-based pagination**: Traditional skip/take approach, suitable for scenarios requiring page numbers

## Features

- Clean separation of pagination logic using function-based components
- Asynchronous enumeration with `IAsyncEnumerable<Movie>`
- Flexible builder pattern for custom pagination contexts
- In-memory SQLite database for easy testing

## Running the Example

```bash
cd samples/EFCorePagination
dotnet run
```

The program will:

1. Create a SQLite database with 100 movies
2. Paginate through all movies using cursor-based pagination (prints titles)
3. Paginate through all movies using offset-based pagination (prints titles)
4. Clean up the database

## Implementation Details

### Cursor-Based Pagination

```csharp
public static IAsyncEnumerable<Movie> GetMoviesPaginatedCursorBased()
{
    var handler = new CursorBased.Composite.PaginationHandlerBuilder<CursorBasedPaginationContext, int?, Movie>()
        .WithPageRetriever(async (paginationContext, cancellationToken) =>
        {
            await using var dbContext = new MoviesDbContext();
            
            var movies = await dbContext.Movies
                .OrderBy(movie => movie.Id)
                .Where(movie => paginationContext == null
                    ? movie.Id <= Limit
                    : movie.Id > paginationContext.Cursor && movie.Id <= paginationContext.Cursor + Limit)
                .ToListAsync(cancellationToken);
            
            return new(movies, movies is [.. , var lastMovie] ? lastMovie.Id : null);
        })
        .WithCursorExtractor(paginationContext => paginationContext.Cursor)
        .WithItemExtractor(paginationContext => paginationContext.Movies)
        .Build();

    return handler.GetAllItemsAsync();
}
```

**Key Points:**

- Uses `movie.Id` as the cursor for efficient range queries
- No need to count total records
- Optimal performance for large datasets

### Offset-Based Pagination

```csharp
public static IAsyncEnumerable<Movie> GetMoviesPaginatedOffsetBased()
{
    var handler = new OffsetBased.Composite.PaginationHandlerBuilder<OffsetBasedPaginationContext, Movie>()
        .WithPageRetriever(async (paginationContext, cancellationToken) =>
        {
            await using var dbContext = new MoviesDbContext();
            
            var total = await dbContext.Movies.CountAsync(cancellationToken);
            
            var movies = await dbContext.Movies
                .OrderBy(movie => movie.Id)
                .Skip(paginationContext?.Offset ?? 0)
                .Take(Limit)
                .ToListAsync(cancellationToken);
            
            return new(movies, (paginationContext?.Offset ?? 0) + movies.Count, total);
        })
        .WithOffsetStateExtractor(paginationContext => new(paginationContext.Offset, paginationContext.Total))
        .WithItemExtractor(paginationContext => paginationContext.Movies)
        .Build();

    return handler.GetAllItemsAsync();
}
```

**Key Points:**

- Uses `Skip()` and `Take()` for traditional paging
- Tracks total count and current offset using `OffsetState`
- Suitable when you need to know total pages or jump to specific pages

## Custom Pagination Contexts

The example defines custom record types to hold pagination state:

```csharp
private record CursorBasedPaginationContext(List<Movie> Movies, int? Cursor);
private record OffsetBasedPaginationContext(List<Movie> Movies, int Offset, int Total);
```

These contexts encapsulate:

- **Data**: The retrieved movies for the current page
- **State**: Cursor position or offset information for the next page

## Use Cases

**Choose cursor-based when:**

- Working with large datasets (thousands+ records)
- Users typically browse sequentially
- Performance is critical
- Data changes frequently (new items added)

**Choose offset-based when:**

- Need to jump to specific pages
- Working with smaller datasets
- UI requires page numbers or "go to page X"
- Data is relatively stable

## Dependencies

- Entity Framework Core
- SQLite provider
- EHonda.Pagination (CursorBased & OffsetBased packages)
