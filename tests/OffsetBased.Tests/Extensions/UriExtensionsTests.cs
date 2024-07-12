using System.Web;
using FluentAssertions;
using NUnit.Framework;
using OffsetBased.Utilities;
using Relaxdays.TestUtilities.Scenarios;

namespace OffsetBased.Tests.Extensions;

[TestOf(typeof(UriExtensions))]
public class UriExtensionsTests
{
    // ----------------------------------------------------------------------------------------------------- //
    //                                          Data                                                         //
    // ----------------------------------------------------------------------------------------------------- //

    public static class QueryParameters
    {
        public record Data(IReadOnlyList<QueryParameter> Parameters);

        public static IEnumerable<Scenario<Data>> Existing =>
        [
            new Data([]).AsScenario(SelectDescription),
            new Data([new("existing_a", "s")]).AsScenario(SelectDescription),
            new Data([new("existing_a", "s"), new("existing_b", "t")]).AsScenario(SelectDescription)
        ];

        public static IEnumerable<Scenario<Data>> Additional =>
        [
            new Data([]).AsScenario(SelectDescription),
            new Data([new("add_a", "x")]).AsScenario(SelectDescription),
            new Data([new("add_a", "x"), new("key_b", "y")]).AsScenario(SelectDescription)
        ];

        private static string SelectDescription(Data data)
        {
            var parameterStrings = data.Parameters.Select(p => $"{p.Name}={p.Value}");
            var parameters = string.Join(", ", parameterStrings);

            return $"[{parameters}]";
        }
    }

    // ----------------------------------------------------------------------------------------------------- //
    //                                   Configuration & Helper                                              //
    // ----------------------------------------------------------------------------------------------------- //

    private static void ParameterShouldExist(Uri uri, QueryParameter parameter)
    {
        var query = HttpUtility.ParseQueryString(uri.Query);

        query.AllKeys.Should().Contain(parameter.Name);
        query.Get(parameter.Name).Should().Be(parameter.Value);
    }

    private static void ParametersShouldExist(Uri uri, IEnumerable<QueryParameter> parameters)
    {
        foreach (var parameter in parameters)
        {
            ParameterShouldExist(uri, parameter);
        }
    }

    private static void ParametersShouldExclusivelyExist(Uri uri, params QueryParameters.Data[] data)
    {
        var query = HttpUtility.ParseQueryString(uri.Query);
        query.AllKeys.Should().HaveCount(data.Sum(d => d.Parameters.Count));

        var allParameters = data.SelectMany(d => d.Parameters);
        ParametersShouldExist(uri, allParameters);
    }

    private static Uri SetupInitialUri(QueryParameters.Data existing)
    {
        const string baseUriString = "https://www.localhost.com";

        var parametersString = string.Join("&", existing.Parameters.Select(p => $"{p.Name}={p.Value}"));

        return existing.Parameters.Count == 0
            ? new(baseUriString)
            : new Uri($"{baseUriString}?{parametersString}");
    }

    // ----------------------------------------------------------------------------------------------------- //
    //                                          Tests                                                        //
    // ----------------------------------------------------------------------------------------------------- //

    [Test]
    [Description(
        """
        Scenario:
            An arbitrary number of parameters are added via `WithAddedOrUpdatedQueryParameters` to a uri that contains
            an arbitrary number of query parameters.

        Expectation:
            The resulting uri contains exclusively the parameters from the original uri and the additional parameters.
        """)]
    public void Additional_query_parameters_are_added(
        [ValueSource(typeof(QueryParameters), nameof(QueryParameters.Existing))]
        Scenario<QueryParameters.Data> existing,
        [ValueSource(typeof(QueryParameters), nameof(QueryParameters.Additional))]
        Scenario<QueryParameters.Data> additional)
    {
        // Arrange
        var uri = SetupInitialUri(existing.Data);

        // Act
        var modifiedUri = uri.WithAddedOrUpdatedQueryParameters(additional.Data.Parameters.ToArray());

        // Assert
        ParametersShouldExclusivelyExist(modifiedUri, existing.Data, additional.Data);
    }

    [Test]
    [Description(
        """
        Scenario:
            An arbitrary number of parameters are added via `WithAddedOrUpdatedQueryParameters` to a uri that contains
            an arbitrary number of query parameters.

        Expectation:
            The original uri is not modified.
        """)]
    public void Original_uri_is_not_modified(
        [ValueSource(typeof(QueryParameters), nameof(QueryParameters.Existing))]
        Scenario<QueryParameters.Data> existing,
        [ValueSource(typeof(QueryParameters), nameof(QueryParameters.Additional))]
        Scenario<QueryParameters.Data> additional)
    {
        // Arrange
        var uri = SetupInitialUri(existing.Data);

        // Act
        _ = uri.WithAddedOrUpdatedQueryParameters(additional.Data.Parameters.ToArray());

        // Assert
        ParametersShouldExclusivelyExist(uri, existing.Data);
    }

    [Test]
    [Description(
        """
        Scenario:
            An arbitrary number of parameters are added via `WithAddedOrUpdatedQueryParameters` to a uri that contains
            an arbitrary number of query parameters. The added parameters contain keys that already exist in the
            original uri.

        Expectation:
            The resulting uri contains exclusively the parameters from the original uri and the additional parameters.
            The values of the existing keys are updated with the values of the added parameters.
        """)]
    public void Existing_keys_are_updated(
        [ValueSource(typeof(QueryParameters), nameof(QueryParameters.Existing))]
        Scenario<QueryParameters.Data> existing)
    {
        // Arrange
        var uri = SetupInitialUri(existing.Data);

        var modifiedValues = new QueryParameters.Data(existing
            .Data
            .Parameters
            .Select(parameter => parameter with { Value = $"{parameter.Value}_modified" })
            .ToList());

        // Act
        var modifiedUri = uri.WithAddedOrUpdatedQueryParameters(modifiedValues.Parameters.ToArray());

        // Assert
        ParametersShouldExclusivelyExist(modifiedUri, modifiedValues);
    }
}
