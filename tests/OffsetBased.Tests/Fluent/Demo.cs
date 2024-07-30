using Core.Fluent;
using OffsetBased.Fluent;

namespace OffsetBased.Tests.Fluent;

public record Number(int Value);

public record Numbers(IReadOnlyList<Number> Values);

// TODO: Replace by a sample project somewhere, add tests
public static class Demo
{
    public static void Run()
    {
        var httpClient = new HttpClient();

        var numbers = httpClient
            .WithPagination()
            .OffsetBased<Numbers, int>()
            // TODO: It's weird that we can call `WithHttpClient` here, we do not want users to be able to do this.
            //       It should only be possible to use the client bound via `WithPagination`.
            .WithItemExtraction(_ => null!)
            .WithPageRequestGeneration(_ => null!)
            .WithPaginationInformationExtraction(_ => null!)
            .WithHttpClient(null!)
            .WithItemExtraction(_ => null!)
            .GetAllItemsAsync();
    }
}
