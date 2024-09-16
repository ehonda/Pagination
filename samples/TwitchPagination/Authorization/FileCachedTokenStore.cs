using System.Text.Json;

namespace TwitchPagination.Authorization;

public class FileCachedTokenStore : ICachedTokenStore
{
    private readonly string _path;

    public FileCachedTokenStore(string path)
    {
        _path = path;
    }

    public async Task PutAsync(CachedToken cachedToken)
    {
        await File.WriteAllTextAsync(_path, JsonSerializer.Serialize(cachedToken));
    }

    public async Task<CachedToken?> GetAsync()
    {
        if (File.Exists(_path) is false)
        {
            return null;
        }

        var json = await File.ReadAllTextAsync(_path);
        return JsonSerializer.Deserialize<CachedToken>(json);
    }
}
