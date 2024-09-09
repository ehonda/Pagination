using JetBrains.Annotations;

namespace Sequential.Composite;

// TODO: Transformed page or page?
[PublicAPI]
public interface IPaginationContextExtractor<in TPage, TPaginationContext>
{
    Task<TPaginationContext> ExtractAsync(TPage page, CancellationToken cancellationToken = default);
}
