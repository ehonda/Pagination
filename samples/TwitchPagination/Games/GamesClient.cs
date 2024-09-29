using System.Text.Json;
using Ardalis.GuardClauses;

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
    
    // Get aoe2
    public async Task<string> GetAgeOfEmpires2()
    {
        // Category on twitch is [0] which we use to search by name here (because it doesn't use the linked IGDB ID)
        //     [0]: https://www.twitch.tv/directory/category/age-of-empires-ii
        var response = await _httpClient.GetAsync("?name=Age of Empires II");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task<GetTopGamesResponse> GetTopGames(int first = 10, string? cursor = null)
    {
        // TODO: Improve query building
        var query = cursor is null
            ? $"?first={first}"
            : $"?first={first}&after={cursor}";
        
        // TODO: Why do we have to specify `games/top...` here? If we just use `top`, the `.../games` suffix of the base
        //       address is eaten.
        var response = await _httpClient.GetAsync($"games/top?{query}");
        response.EnsureSuccessStatusCode();

        // TODO: When can this be null? Can we just bang it away?
        return (await JsonSerializer.DeserializeAsync<GetTopGamesResponse>(
                            await response.Content.ReadAsStreamAsync(),
                            new JsonSerializerOptions(JsonSerializerDefaults.Web)))!;
    }
    
    public async Task<List<string>> GetTopGamesNames(int first = 10)
    {
        Guard.Against.OutOfRange(first, nameof(first), 1, 100);
        
        // TODO: Why do we have to specify `games/top...` here? If we just use `top`, the `.../games` suffix of the base
        //       address is eaten.
        var response = await _httpClient.GetAsync($"games/top?first={first}");
        response.EnsureSuccessStatusCode();

        var names = (await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync()))
            .RootElement
            .GetProperty("data")
            .EnumerateArray()
            // TODO: When can this be null? Can we just bang it away?
            .Select(game => game.GetProperty("name").GetString()!)
            .ToList();
        
        return names;
    }
}
