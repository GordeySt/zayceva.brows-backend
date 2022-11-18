namespace Application.Common.Constants;

public static class NotFoundExceptionMessageConstants
{
    public const string NotFoundItem = nameof(NotFoundItem);
}

public static class BadRequestExceptionMessageConstants
{
    public const string ExistedItem = nameof(ExistedItem);
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
