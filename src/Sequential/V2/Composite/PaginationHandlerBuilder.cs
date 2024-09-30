using JetBrains.Annotations;

namespace Sequential.V2.Composite;

[PublicAPI]
public static class PaginationHandlerBuilder
{
    public static PaginationHandlerBuilder<TPaginationContext, TItem> Create<TPaginationContext, TItem>()
        where TPaginationContext : class
        => new();
}
