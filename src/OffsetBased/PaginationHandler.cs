using System.Numerics;
using Sequential;

namespace OffsetBased;

// TODO: Improve naming for `TNumber`
public abstract class PaginationHandler<TPaginationContext, TIndex, TItem>
    : PaginationHandler<TPaginationContext, TItem>
    where TPaginationContext : class
    where TIndex : INumber<TIndex>
{
    protected override async Task<bool> NextPageExistsAsync(
            TPaginationContext context,
            CancellationToken cancellationToken = default)
    {
        var indexData = await ExtractIndexDataAsync(context, cancellationToken);
            
        // TODO: Is `<` the correct choice here?
        return indexData.Offset < indexData.Total;
    }

    // TODO: Should this be one method, like `ExtractOffsetAndTotalAsync`?
    protected abstract Task<IndexData<TIndex>> ExtractIndexDataAsync(
        TPaginationContext context,
        CancellationToken cancellationToken = default);
}
