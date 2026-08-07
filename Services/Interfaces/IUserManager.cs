using Auditor.Models;

namespace Auditor.Services.Interfaces;

public enum ErrType
{
    USER_NOT_FOUND = 0,
    PASSWORD_MISMATCH = 1,
    EXISTING_EMAIL_FOUND = 2
}

public class UserErr
{
    public ErrType Type;
    public string ErrMessage = string.Empty;
}

public class UserResults
{
    public bool isSuccess;
    public UserErr? Err { get; init; }
    public User? user;

    public static UserResults Ok(User user)
    {
        return new UserResults
        {
            isSuccess = true,
            Err = null,
            user = user
        };
    }

    public static UserResults Error(ErrType errType, string message)
    {
        UserErr err = new()
        {
            Type = errType,
            ErrMessage = message
        };
        return new UserResults
        {
            isSuccess = false,
            Err = err,
            user = null
        };
    }
}


public interface IUserManager
{
    public Task<UserResults> AuthenticateUser(string email, string password);

    public Task<UserResults> SignupUser(string name, string email, string password);

    public Task ResetUserpassword(string resetToken, string password);
}