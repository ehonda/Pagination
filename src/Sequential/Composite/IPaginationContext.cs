namespace Sequential.Composite;

public interface IPaginationContext<out TPage>
{
    TPage CurrentPage { get; }
    
    Task<bool> NextPageExistsAsync(CancellationToken cancellationToken = default);
}
