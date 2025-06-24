using TestUtilities.Games;

namespace OffsetBased.TUnit.Tests.TestUtilities;

public record Page(IReadOnlyList<string> Items, Pagination Pagination);
