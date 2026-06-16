using System.Net;

namespace MarketHub.Application.Responses;

public class BaseResponse
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public List<string>? ValidationErrors { get; set; }

    public BaseResponse()
    {
        Success = true;
        StatusCode = (int)HttpStatusCode.OK;
    }

    public BaseResponse(bool success, int statusCode = (int)HttpStatusCode.OK)
    {
        Success = success;
        StatusCode = statusCode;
    }

    public BaseResponse(string message)
    {
        Success = true;
        StatusCode = 200;
        Message = message;
    }

    public BaseResponse(bool success, string message, int statusCode = (int)HttpStatusCode.OK)
    {
        Success = success;
        Message = message;
        StatusCode = statusCode;
    }
}