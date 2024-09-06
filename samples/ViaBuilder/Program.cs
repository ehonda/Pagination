using Core;
using OffsetBased.Fluent;
using ViaBuilder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
// TODO: Fix these stubs
// TODO: This is not actually a 1-1 comparison as here we're using `OffsetBasedPaginationHandler` and in the other project
//       we're implementing `SequentialPaginationHandler` directly (which are somewhat different concepts, as offset
//       based pagination handler has some assumptions resulting in a different API)
builder.Services.AddTransient<IPaginationHandler<Issue>>(serviceProvider => OffsetBasedPaginationHandlerBuilder
    .Create<HttpResponseMessage, Issue>()
    .WithHttpClient(serviceProvider.GetRequiredService<HttpClient>())
    .WithPageRequestGeneration(generation => generation.ByRequestFactory(
        () => new(),
        new(
            "page",
            "per_page",
            30)))
    .WithPaginationInformationExtraction(extraction => extraction.ByFunction(
        response => new(response, new(0, 0))))
    .WithItemExtraction(extraction => extraction
        .ByAsyncFunction(async (response, cancellationToken) =>
        {
            var issues = await response.Content.ReadFromJsonAsync<Issue[]>(cancellationToken);
            return (issues ?? []).ToAsyncEnumerable();
        }))
    .Build());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

await app.RunAsync();
