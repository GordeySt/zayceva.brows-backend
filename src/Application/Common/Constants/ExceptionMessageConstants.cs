namespace Application.Common.Constants;

public static class NotFoundExceptionMessageConstants
{
    public const string NotFoundUserMessage = "User with the following id not found";
}

public static class BadRequestExceptionMessageConstants
{
    public const string ExistedUserMessage = "User with the following email already exists";
    public const string ProblemVerifyingEmailMessage = "Problem verifying message";
}

public static class InternalServerErrorExceptionMessageConstants
{
    public const string ProblemCreatingUser = "Problem creating user";
}
