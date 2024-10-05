using System.Diagnostics.CodeAnalysis;

namespace Deprecated.OffsetBased.PageRequestGenerationHandling.Fluent;

/// <summary>
/// Fluent API for providing an <see cref="IPageRequestGenerator"/>.
/// </summary>
/// <remarks>
/// This class is not static despite only providing static methods to provide a nicer interface in terms of both
/// discoverability via IDE (by the builders taking functions using an instance of this class as an argument) as well as
/// extensibility (by allowing to add extension methods to this class).
/// </remarks>
[SuppressMessage(
    "SonarLint",
    "S1118: Utility classes should not have public constructors",
    Justification = "Discoverability and extensibility, see xml doc remarks.")]
[SuppressMessage(
    "Roslyn",
    "CA1822: Mark members as static",
    Justification = "Making the methods non-static helps resolve ambiguous function calls.")]
[PublicAPI]
public class PageRequestGeneration
{
    /// <summary>
    /// Fluent API for providing a <see cref="PageRequestGeneratorViaResource"/>.
    /// </summary>
    /// <param name="resource">The resource to generate the base request for.</param>
    /// <param name="paginationParametersConfiguration">The configuration for the pagination parameters.</param>
    /// <returns>The page request generator.</returns>
    public PageRequestGeneratorViaResource ByResource(
        string resource,
        PaginationParametersConfiguration paginationParametersConfiguration)
        => new(resource, paginationParametersConfiguration);

    /// <summary>
    /// Fluent API for providing a <see cref="PageRequestGeneratorViaInitialRequest"/>.
    /// </summary>
    /// <param name="initialRequest">The initial request to clone for each page.</param>
    /// <param name="paginationParametersConfiguration">The configuration for the pagination parameters.</param>
    /// <returns>The page request generator.</returns>
    public PageRequestGeneratorViaInitialRequest ByInitialRequest(
        HttpRequestMessage initialRequest,
        PaginationParametersConfiguration paginationParametersConfiguration)
        => new(initialRequest, paginationParametersConfiguration);

    /// <summary>
    /// Fluent API for providing a <see cref="PageRequestGeneratorViaRequestFactory"/>.
    /// </summary>
    /// <param name="makeRequest">The factory to create the base request.</param>
    /// <param name="paginationParametersConfiguration">The configuration for the pagination parameters.</param>
    /// <returns>The page request generator.</returns>
    public PageRequestGeneratorViaRequestFactory ByRequestFactory(
        Func<HttpRequestMessage> makeRequest,
        PaginationParametersConfiguration paginationParametersConfiguration)
        => new(makeRequest, paginationParametersConfiguration);

    /// <summary>
    /// Fluent API for providing a <see cref="PageRequestGeneratorViaAsyncRequestFactory"/>.
    /// </summary>
    /// <param name="makeRequestAsync">The async factory to create the base request.</param>
    /// <param name="paginationParametersConfiguration">The configuration for the pagination parameters.</param>
    /// <returns>The page request generator.</returns>
    public PageRequestGeneratorViaAsyncRequestFactory ByAsyncRequestFactory(
        Func<CancellationToken, Task<HttpRequestMessage>> makeRequestAsync,
        PaginationParametersConfiguration paginationParametersConfiguration)
        => new(makeRequestAsync, paginationParametersConfiguration);
}
