namespace Application.Common.Enums;

public enum ApplicationResultType
{
    Success = 200,
    InvalidData = 400,
    Unauthorized = 401,
    NotFound = 404,
    InternalError = 500,
}