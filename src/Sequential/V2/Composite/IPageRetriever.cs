using JetBrains.Annotations;

namespace Sequential.V2.Composite;

[PublicAPI]
public interface IPageRetriever<TPaginationContext>
{
    Task<TPaginationContext> GetAsync(TPaginationContext? context, CancellationToken cancellationToken = default);
}
