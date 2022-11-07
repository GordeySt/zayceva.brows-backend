namespace Application.Common.Models;

public class IdentityResult
{
    private IdentityResult(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    public bool Succeeded { get; set; }

    public string[] Errors { get; set; }

    public static IdentityResult Success()
    {
        return new IdentityResult(true, Array.Empty<string>());
    }
    
    public static IdentityResult Failure()
    {
        return new IdentityResult(false, Array.Empty<string>());
    }

    public static IdentityResult Failure(IEnumerable<string> errors)
    {
        return new IdentityResult(false, errors);
    }
}