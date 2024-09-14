using System.Net.Http.Json;
using System.Text.Json;
using TwitchPagination;

var camelCaseOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

var snakeCaseLowerOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
};

var clientData = (await JsonSerializer.DeserializeAsync<ClientData>(
    File.OpenRead("client.json"),
    camelCaseOptions))!;

var client = new HttpClient();

// See: https://dev.twitch.tv/docs/api/get-started/#get-an-oauth-token
var oauthResponse = await client
    .PostAsync("https://id.twitch.tv/oauth2/token", new FormUrlEncodedContent(new Dictionary<string, string>
    {
        ["client_id"] = clientData.Id,
        ["client_secret"] = clientData.Secret,
        ["grant_type"] = "client_credentials"
    }));

var tokenResponse = (await oauthResponse.Content.ReadFromJsonAsync<TokenResponse>(snakeCaseLowerOptions))!;

Console.WriteLine($"Access token: {tokenResponse.AccessToken}");
Console.WriteLine($"Expires in: {tokenResponse.ExpiresIn}");
Console.WriteLine($"Token type: {tokenResponse.TokenType}");
