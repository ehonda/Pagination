using EFCorePagination;

await Functions.SetupDatabase();

await foreach (var movie in Functions.GetMoviesPaginatedCursorBased())
{
    Console.WriteLine(movie.Title);
}

await foreach (var movie in Functions.GetMoviesPaginatedOffsetBased())
{
    Console.WriteLine(movie.Title);
}

await Functions.TearDownDatabase();
