namespace Application.Common.Constants;

public static class NotFoundExceptionMessageConstants
{
    public const string NotFoundUser = nameof(NotFoundUser);
}

public static class BadRequestExceptionMessageConstants
{
    public const string ExistedUser = nameof(ExistedUser);
    public const string ProblemVerifyingEmail = nameof(ProblemVerifyingEmail);
}

public static class UnauthorizedExceptionMessageConstants
{
    public const string InvalidPassword = nameof(InvalidPassword);
} 

public static class InternalServerErrorExceptionMessageConstants
{
    public const string ProblemCreatingUser = nameof(ProblemCreatingUser);
}
