using System.Net.Http.Json;
using System.Text.Json;
using Ardalis.GuardClauses;
using CursorBased.Composite;
using TwitchPagination.Games.Composite;

namespace TwitchPagination.Games;

public class GamesClient
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

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
    
    public IAsyncEnumerable<Game> GetAllTopGames(int first = 10)
    {
        var paginationHandler = new PaginationHandlerBuilder<GetTopGamesResponse, string, Game>()
            .WithPageRetriever(new PageRetriever(this))
            .WithCursorExtractor(new CursorExtractor())
            .WithItemExtractor(new ItemExtractor())
            .Build();

        return paginationHandler.GetAllItemsAsync();
    }
    
    /// <summary>
    /// <b>📖 EXAMPLE:</b> Direct usage of the <see cref="PaginationHandler{TPaginationContext, TCursor, TItem}">
    /// CursorBased.Composite.PaginationHandler</see> by using the
    /// <see cref="PaginationHandlerBuilder{TPaginationContext, TCursor, TItem}">builder</see> and specifying the
    /// components via lambdas.
    /// </summary>
    public IAsyncEnumerable<Game> GetAllTopGamesByFunctions(int first = 10)
    {
        // TODO: Nicer pagination handler builder creation
        var paginationHandler = new PaginationHandlerBuilder<GetTopGamesResponse, Game>()
            .WithPageRetriever((context, _) => GetTopGames(100, context?.Pagination.Cursor))
            .WithCursorExtractor(context => context.Pagination.Cursor)
            .WithItemExtractor(context => context.Data)
            .Build();

        return paginationHandler.GetAllItemsAsync();
    }
    
    public async Task<GetTopGamesResponse> GetTopGames(int first = 10, string? cursor = null)
    {
        Guard.Against.OutOfRange(first, nameof(first), 1, 100);

        // TODO: Improve query building
        var query = cursor is null
            ? $"?first={first}"
            : $"?first={first}&after={cursor}";
        
        // TODO: Why do we have to specify `games/top...` here? If we just use `top`, the `.../games` suffix of the base
        //       address is eaten.
        var response = await _httpClient.GetAsync($"games/top?{query}");
        response.EnsureSuccessStatusCode();

        // TODO: When can this be null? Can we just bang it away?
        return (await response.Content.ReadFromJsonAsync<GetTopGamesResponse>(JsonSerializerOptions))!;
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
