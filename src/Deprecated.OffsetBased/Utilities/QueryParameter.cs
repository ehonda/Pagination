namespace Deprecated.OffsetBased.Utilities;

/// <summary>
/// Represents a query parameter for a URL.
/// </summary>
/// <param name="Name">The name of the query parameter.</param>
/// <param name="Value">The value of the query parameter.</param>
[PublicAPI]
public record QueryParameter(string Name, string Value);
