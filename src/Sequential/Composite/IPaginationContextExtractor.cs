namespace Sequential.Composite;

public interface IPaginationContextExtractor<in TPage, TPaginationContext>
{
    Task<TPaginationContext> ExtractAsync(TPage page, CancellationToken cancellationToken = default);
}
