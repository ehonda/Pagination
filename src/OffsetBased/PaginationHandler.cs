using System.Numerics;
using Sequential;

namespace OffsetBased;

public abstract class PaginationHandler<TPaginationContext, TNumber, TItem>
    : PaginationHandler<TPaginationContext, TItem>
    where TPaginationContext : class
    where TNumber : INumber<TNumber>
{
    protected override async Task<bool> NextPageExistsAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default)
        // TODO: Is `<` the correct choice here?
        => await ExtractOffsetAsync(context) < await ExtractTotalAsync(context);

    protected abstract Task<TNumber> ExtractOffsetAsync(TPaginationContext context);
    
    protected abstract Task<TNumber> ExtractTotalAsync(TPaginationContext context);
}
