namespace MarketHub.Application.Shared;

public class MetaData
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }

    public bool HasPrevious => 1 < CurrentPage;
    public bool HasNext => CurrentPage < TotalPages;
}