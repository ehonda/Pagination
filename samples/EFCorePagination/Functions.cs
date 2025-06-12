using Microsoft.EntityFrameworkCore;

namespace EFCorePagination;

public static class Functions
{
    public static async Task SetupDatabase()
    {
        await using var context = new MoviesDbContext();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        var movies = Enumerable
            .Range(0, 100)
            .Select(i => new Movie { Title = $"Movie {i}" });

        await context.Movies.AddRangeAsync(movies);
        await context.SaveChangesAsync();
    }

    public static async Task TearDownDatabase()
    {
        await using var context = new MoviesDbContext();

        await context.Database.EnsureDeletedAsync();
    }

    private const int Limit = 10;

    private record CursorBasedPaginationContext(List<Movie> Movies, int? Cursor);

    public static IAsyncEnumerable<Movie> GetMoviesPaginatedCursorBased()
    {
        // TODO: Improve used types
        // TODO: Improve user experience
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

                return new(movies, movies is [.., var lastMovie] ? lastMovie.Id : null);
            })
            .WithCursorExtractor(paginationContext => paginationContext.Cursor)
            .WithItemExtractor(paginationContext => paginationContext.Movies)
            .Build();

        return handler.GetAllItemsAsync();
    }

    private record OffsetBasedPaginationContext(List<Movie> Movies, int Offset, int Total);

    public static IAsyncEnumerable<Movie> GetMoviesPaginatedOffsetBased()
    {
        var handler = new OffsetBased.Composite.PaginationHandlerBuilder<OffsetBasedPaginationContext, Movie>()
            .WithPageRetriever(async (paginationContext, cancellationToken) =>
            {
                await using var dbContext = new MoviesDbContext();

                // TODO: Very inefficient to do this every time, we should do it up front
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
}
