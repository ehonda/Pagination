namespace TwitchPagination.Games;

/// <summary>
/// Response dto for `games/top` endpoint.
/// </summary>
/// <remarks>
/// Example (taken from the <a href="https://dev.twitch.tv/docs/api/reference/#get-top-games">documentation</a>)
/// response looks like this:
/// <code>
///     {
///       "data": [
///         {
///           "id": "493057",
///           "name": "PUBG: BATTLEGROUNDS",
///           "box_art_url": "https://static-cdn.jtvnw.net/ttv-boxart/493057-{width}x{height}.jpg",
///           "igdb_id": "27789"
///         },
///         ...
///       ],
///       "pagination": { "cursor": "eyJiIjpudWxsLCJhIjp7Ik9mZnNldCI6MjB9fQ==" }
///     }
/// </code>
/// </remarks>
public record GetTopGamesResponse(
    IReadOnlyList<Game> Data,
    Pagination Pagination);

/// <summary>
/// Represents a game.
/// </summary>
public record Game(
    string Id,
    string Name,
    string BoxArtUrl,
    string IgdbId);

/// <summary>
/// Represents the pagination data.
/// </summary>
public record Pagination(
    string Cursor);
