using System.Net.Http.Json;
using System.Text.Json;
using TwitchPagination;

var clientData = (await JsonSerializer.DeserializeAsync<ClientData>(File.OpenRead("client.json")))!;

var tokenResponse = await Functions.GetAccessTokenResponse(clientData);

var client = new HttpClient();

// Setup default headers
client.DefaultRequestHeaders.Authorization = new("Bearer", tokenResponse.AccessToken);
client.DefaultRequestHeaders.Add("Client-Id", clientData.Id);

// IGDB ID: https://www.igdb.com/games/age-of-empires-ii-hd-edition
var queryParam = "igdb_id=2950";
var aoe2HdResponse = await client.GetAsync($"https://api.twitch.tv/helix/games?{queryParam}");

var doc = await JsonDocument.ParseAsync(await aoe2HdResponse.Content.ReadAsStreamAsync());
// TODO: Why does this not work?
// var writer = new Utf8JsonWriter(Console.OpenStandardOutput());
// doc.WriteTo(writer);
