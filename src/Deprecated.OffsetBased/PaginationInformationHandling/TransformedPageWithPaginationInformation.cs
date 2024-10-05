namespace Deprecated.OffsetBased.PaginationInformationHandling;

/// <summary>
/// Represents a transformed page with extracted pagination information.
/// </summary>
/// <param name="TransformedPage">The transformed page.</param>
/// <param name="PaginationInformation">The extracted pagination information.</param>
/// <typeparam name="TTransformedPage">The type of the transformed page.</typeparam>
[PublicAPI]
public record TransformedPageWithPaginationInformation<TTransformedPage>(
    TTransformedPage TransformedPage,
    PaginationInformation PaginationInformation);
