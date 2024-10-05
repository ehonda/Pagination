using JetBrains.Annotations;

namespace SpotifyPagination;

// We need a class with a paramless ctor instead to use it with IOptions
// See: https://learn.microsoft.com/en-us/dotnet/core/extensions/options#options-class
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class ClientData
{
    public const string ConfigurationSectionName = nameof(ClientData);

    public string Id { get; set; } = null!;

    public string Secret { get; set; } = null!;
}
