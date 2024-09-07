namespace Sequential.Composite;

public class PaginationContext<TPage> : IPaginationContext<TPage>
{
    public PaginationContext(TPage currentPage, bool nextPageExists)
    {
        CurrentPage = currentPage;
        NextPageExists = nextPageExists;
    }

    public TPage CurrentPage { get; }
    
    public bool NextPageExists { get; }
}
