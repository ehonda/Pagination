namespace TestUtilities.Games;

public record GetTopGamesResponse(
    IReadOnlyList<string> Games,
    string? Cursor);
