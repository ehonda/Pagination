using JetBrains.Annotations;

namespace Core.Fluent;

/// <summary>
/// Represents the entry point for setting up a pagination handler via fluent api.
/// </summary>
/// <remarks>
/// To define a fluent pagination handler, create an extension method returning an instance of
/// <see cref="PaginationHandlerBuilder{TItem}"/> on this class, binding the builder to the <see cref="HttpClient"/>
/// used in this setup via <see cref="PaginationHandlerBuilder{TItem}.WithHttpClient"/>.
/// </remarks>
[PublicAPI]
public class PaginationHandlerBuilderSetup
{
    /// <summary>
    /// The http client to bind the pagination handler builder to.
    /// </summary>
    public required HttpClient HttpClient { get; init; }
}
