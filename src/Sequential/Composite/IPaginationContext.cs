using JetBrains.Annotations;

namespace Sequential.Composite;

[PublicAPI]
public interface IPaginationContext<out TPage>
{
    TPage CurrentPage { get; }
    
    bool NextPageExists { get; }
}
