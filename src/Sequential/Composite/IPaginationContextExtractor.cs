namespace Sequential.Composite;

// TODO: Transformed page or page?
public interface IPaginationContextExtractor<in TPage, TTransformedPage>
{
    Task<IPaginationContext<TTransformedPage>> ExtractAsync(TPage page, CancellationToken cancellationToken = default);
}
