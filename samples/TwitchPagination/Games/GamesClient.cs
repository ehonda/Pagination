namespace TwitchPagination.Games;

public class GamesClient
{
    private readonly HttpClient _httpClient;

    public GamesClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // TODO: Generic methods
    public async Task<string> GetFortnite()
    {
        var response = await _httpClient.GetAsync("?name=Fortnite");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}
