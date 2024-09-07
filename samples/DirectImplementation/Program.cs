using Core;
using DirectImplementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddHttpClient<GitHubOriginalPaginationHandler<Issue>>();
builder.Services.AddTransient<IPaginationHandler<Issue>>(serviceProvider => new GitHubOriginalPaginationHandler<Issue>(
    // TODO: This is the wrong client, we want the typed one
    serviceProvider.GetRequiredService<HttpClient>(),
    async (response, cancellationToken) =>
    {
        var issues = await response.Content.ReadFromJsonAsync<Issue[]>(cancellationToken: cancellationToken);
        return (issues ?? []).ToAsyncEnumerable();
    }));

builder.Services.AddHttpClient<GitHubOriginalPaginationHandler<PullRequest>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

await app.RunAsync();

