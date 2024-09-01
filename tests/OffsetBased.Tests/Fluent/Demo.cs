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

    public static void Problem_direct_handler_usage()
    {
        var httpClient = new HttpClient();

        var handler = httpClient
            .WithPagination()
            .OffsetBased<Numbers, int>()
            .Build();

        // Now we can't do this as it produces the following error message:
        //     Method 'GetAllItemsAsync' has 2 parameter(s) but is invoked with 0 argument(s)
        // var numbers = handler.GetAllItemsAsync();
        
        // Instead we have to do this which forces us to specify the HttpClient again
        var numbers = handler.GetAllItemsAsync(httpClient);

        // And we can even do this which can lead to bugs if the first HttpClient is configured differently
        var anotherHttpClient = new HttpClient();
        var anotherNumbers = handler.GetAllItemsAsync(anotherHttpClient);
    }

    public static void Problem_Multiple_WithHttpClient_calls()
    {
        var httpClient = new HttpClient();
        var anotherHttpClient = new HttpClient();

        // This is confusing and should not be possible
        var numbers = httpClient
            .WithPagination()
            .OffsetBased<Numbers, int>()
            .WithHttpClient(httpClient)
            .WithHttpClient(anotherHttpClient)
            .GetAllItemsAsync();
    }
}
