using System.Text.Json;

namespace SpotifyPagination.Artists;

public class ArtistsClient
{
    private const string GraceJonesId = "2f9ZiYA2ic1r1voObUimdd";

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
        var response = await _httpClient.GetAsync($"artists/{GraceJonesId}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task<List<string>> GetGraceJonesAlbumNames(int limit = 10)
    {
        // TODO: Why do we have to specify `artits/{id}...` here? If we just use `{id}`, the `.../artists` suffix of the
        //       base address is eaten.
        var response = await _httpClient.GetAsync($"artists/{GraceJonesId}/albums?limit={limit}");
        response.EnsureSuccessStatusCode();

        var names = (await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync()))
            .RootElement
            .GetProperty("items")
            .EnumerateArray()
            .Select(album => album.GetProperty("name").GetString()!)
            .ToList();
        
        return names;
    }
}
