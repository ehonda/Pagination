using JetBrains.Annotations;

namespace Sequential.Composite.PageRetrievers;

[PublicAPI]
public interface IPageRetriever<TPaginationContext>
{
    Task<TPaginationContext> GetAsync(TPaginationContext? context, CancellationToken cancellationToken = default);
}
