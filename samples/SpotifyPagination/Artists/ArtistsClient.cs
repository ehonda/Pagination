using System.Text.Json;

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
        const string graceJonesId = "2f9ZiYA2ic1r1voObUimdd";
        
        // TODO: Why do we have to specify `artits/{id}...` here? If we just use `{id}`, the `.../artists` suffix of the
        //       base address is eaten.
        var response = await _httpClient.GetAsync($"artists/{graceJonesId}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}
