using System.Net.Http.Json;
using System.Text.Json;
using Ardalis.GuardClauses;

namespace SpotifyPagination.Artists;

public class ArtistsClient
{

    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly HttpClient _httpClient;

    public ArtistsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetGraceJones()
    {
        // TODO: Why do we have to specify `artits/{id}...` here? If we just use `{id}`, the `.../artists` suffix of the
        //       base address is eaten.
        var response = await _httpClient.GetAsync($"artists/{Ids.GraceJones}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task<List<string>> GetGraceJonesAlbumNames(int limit = 10)
    {
        // TODO: Why do we have to specify `artits/{id}...` here? If we just use `{id}`, the `.../artists` suffix of the
        //       base address is eaten.
        var response = await _httpClient.GetAsync($"artists/{Ids.GraceJones}/albums?limit={limit}");
        response.EnsureSuccessStatusCode();

        var names = (await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync()))
            .RootElement
            .GetProperty("items")
            .EnumerateArray()
            .Select(album => album.GetProperty("name").GetString()!)
            .ToList();
        
        return names;
    }
    
    // TODO: Make it harder to accidentally switch the order of `limit` and `offset`
    public async Task<GetAlbumsResponse> GetAlbums(string artistId, int limit = 10, int offset = 0)
    {
        Guard.Against.OutOfRange(limit, nameof(limit), 1, 50);
        Guard.Against.Negative(offset);
        
        var response = await _httpClient.GetAsync($"artists/{artistId}/albums?limit={limit}&offset={offset}");
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<GetAlbumsResponse>(JsonSerializerOptions))!;
    }
}
