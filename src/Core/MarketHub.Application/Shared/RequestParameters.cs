namespace MarketHub.Application.Shared;

public class RequestParameters
{
    private const int MAX_PAGE_SIZE = 50;
    private int _pageSize = 10;
    private int _pageNumber = 1;

    public int PageNumber
    {
        get
        {
            return _pageNumber;
        }
        set
        {
            _pageNumber = value < 1 ? 1 : value;
        }
    }

    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value;
        }
    }
}