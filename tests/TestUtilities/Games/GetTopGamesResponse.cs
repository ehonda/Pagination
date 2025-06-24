namespace TestUtilities.Games;

public record GetTopGamesResponse(
    IReadOnlyList<string> Data,
    Pagination Pagination);

public record Pagination(
    string? Cursor);
