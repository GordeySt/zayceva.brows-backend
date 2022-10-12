using Application.Common.Enums;

namespace Application.Common.Models;

public class ApplicationResult
{
    public ApplicationResultType Result { get; set; }
    
    public string? Message { get; set; }
    
    public bool IsResultFailed => this.Result != ApplicationResultType.Success;
    
    public ApplicationResult(ApplicationResultType result)
    {
        Result = result;
    }
    
    public ApplicationResult(ApplicationResultType result, string message)
        : this(result)
    {
        Message = message;
    }
    
    protected ApplicationResult() { }
}

public class ApplicationResult<T> : ApplicationResult
{
    public T? Data { get; set; }
    
    public ApplicationResult() { }
    
    public ApplicationResult(ApplicationResultType result)
    {
        Result = result;
    }
    
    public ApplicationResult(T data)
    {
        Result = ApplicationResultType.Success;
        Data = data;
    }
    
    public ApplicationResult(ApplicationResultType result, string message)
        : this(result)
    {
        Message = message;
    }
    
    public ApplicationResult(ApplicationResultType result, T data)
        : this(result)
    {
        Data = data;
    }
    
    public ApplicationResult(ApplicationResultType result, string message, T data)
        : this(result)
    {
        Message = message;
        Data = data;
    }
}