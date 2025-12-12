namespace CompanyRatingFrontend.Data;

public class PagedList<T>(List<T> content, int page, int size, int total)
{
    public List<T> Content { get; } = content;
    public int Page { get; } = page;
    public int Size { get; } = size;
    public int Total { get; } = total;
    public bool HasNext => Page * Size < Total;
    public bool HasPrev => Page > 1;

    public static PagedList<T> Empty => new([], 0, 0, 0);
}