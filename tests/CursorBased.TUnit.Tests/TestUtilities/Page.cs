namespace CursorBased.TUnit.Tests.TestUtilities;

public record Page(
    IReadOnlyList<string> Data,
    Pagination Pagination);
