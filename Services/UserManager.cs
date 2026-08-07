using Auditor.Services.Interfaces;
using Auditor.Models;
using Auditor.Data;
using Microsoft.EntityFrameworkCore;

public class UserManager : IUserManager
{
    private readonly AuditorDb _db;

    public UserManager(AuditorDb db)
    {
        _db = db;
    }

    public static string HashPassword(string passwordRaw)
    {
        //Stub method. TODO: Implement BCrypt.NET
        return passwordRaw;
    }

    public static bool CheckPasswordMatch(User user, string passwordRaw)
    {

        //TODO Implement BCrypt.NET Password validation.
        if (user.PasswordHash == passwordRaw)
        {
            return true;
        }
        return false;
    }

    public async Task<UserResults> AuthenticateUser(string email, string password)
    {
        User? destUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (destUser == null)
        {
            return UserResults.Error(ErrType.USER_NOT_FOUND, "Unable to find user.");
        }
        if (!CheckPasswordMatch(destUser, password))
        {
            return UserResults.Error(ErrType.PASSWORD_MISMATCH, "Invalid Credentals");
        }
        return UserResults.Ok(destUser);
    }

    public Task ResetUserpassword(string resetToken, string password)
    {
        throw new NotImplementedException();
    }

    public Task<UserResults> SignupUser(string email, string password)
    {
        throw new NotImplementedException();
    }
}