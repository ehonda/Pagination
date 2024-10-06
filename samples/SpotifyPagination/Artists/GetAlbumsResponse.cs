namespace SpotifyPagination.Artists;

/// <summary>
/// Response dto for `artists/{id}/albums` endpoint.
/// </summary>
/// <remarks>
/// Example
/// <code>
///   {
///     "href": "https://api.spotify.com/v1/me/shows?offset=0&limit=20",
///     "limit": 20,
///     "next": "https://api.spotify.com/v1/me/shows?offset=1&limit=1",
///     "offset": 0,
///     "previous": "https://api.spotify.com/v1/me/shows?offset=1&limit=1",
///     "total": 4,
///     "items": [
///       {
///         "album_type": "compilation",
///         "total_tracks": 9,
///         "available_markets": [
///           "CA",
///           "BR",
///           "IT"
///         ],
///         "external_urls": {
///           "spotify": "string"
///         },
///         "href": "string",
///         "id": "2up3OPMp9Tb4dAKM2erWXQ",
///         "images": [
///           {
///             "url": "https://i.scdn.co/image/ab67616d00001e02ff9ca10b55ce82ae553c8228",
///             "height": 300,
///             "width": 300
///           }
///         ],
///         "name": "string",
///         "release_date": "1981-12",
///         "release_date_precision": "year",
///         "restrictions": {
///           "reason": "market"
///         },
///         "type": "album",
///         "uri": "spotify:album:2up3OPMp9Tb4dAKM2erWXQ",
///         "artists": [
///           {
///             "external_urls": {
///               "spotify": "string"
///             },
///             "href": "string",
///             "id": "string",
///             "name": "string",
///             "type": "artist",
///             "uri": "string"
///           }
///         ],
///         "album_group": "compilation"
///       }
///     ]
///   }
/// </code>
/// </remarks>
public record GetAlbumsResponse(
    string Href,
    int Limit,
    string? Next,
    int Offset,
    string? Previous,
    int Total,
    IReadOnlyList<Album> Items);

/// <summary>
/// Represents an album.
/// </summary>
public record Album(
    string AlbumType,
    int TotalTracks,
    IReadOnlyList<string> AvailableMarkets,
    ExternalUrls ExternalUrls,
    string Href,
    string Id,
    IReadOnlyList<Image> Images,
    string Name,
    string ReleaseDate,
    string ReleaseDatePrecision,
    Restrictions Restrictions,
    string Type,
    string Uri,
    IReadOnlyList<Artist> Artists,
    string AlbumGroup);

/// <summary>
/// Represents the external urls.
/// </summary>
public record ExternalUrls(
    string Spotify);

/// <summary>
/// Represents an image.
/// </summary>
public record Image(
    string Url,
    int Height,
    int Width);

/// <summary>
/// Represents a restrictions.
/// </summary>
public record Restrictions(
    string Reason);

/// <summary>
/// Represents an artist.
/// </summary>
public record Artist(
    ExternalUrls ExternalUrls,
    string Href,
    string Id,
    string Name,
    string Type,
    string Uri);
