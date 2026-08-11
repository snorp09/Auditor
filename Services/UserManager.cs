using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Auditor.Services.Interfaces;
using Auditor.Models;
using Auditor.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Routing;

public class UserManager : IUserManager
{
    private readonly JwtConfig _jwtConfig;
    private readonly IEmailService _emailService;
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AuditorDb _db;

    public UserManager(AuditorDb db, IOptions<JwtConfig> jwtConfig, IEmailService emailService, LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _jwtConfig = jwtConfig.Value;
        _emailService = emailService;
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
    }

    public static string HashPassword(string passwordRaw)
    {
        return BCrypt.Net.BCrypt.HashPassword(passwordRaw);
    }

    public static bool CheckPasswordMatch(User user, string passwordRaw)
    {
        return BCrypt.Net.BCrypt.Verify(passwordRaw, user.PasswordHash);
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

    public async Task GeneratePasswordResetToken(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        User? user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return;
        }
        if (_httpContextAccessor.HttpContext == null)
        {
            throw new Exception("HttpContext is null");
        }

        string resetKey = _jwtConfig.SecretKey + user.PasswordHash;
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(resetKey));
        var resetTokenDesc = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Purpose", "Password Reset")
            }),
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
        };
        var resetToken = tokenHandler.CreateToken(resetTokenDesc);
        var webToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(tokenHandler.WriteToken(resetToken)));
        var actionURL = _linkGenerator.GetUriByPage(
            httpContext: _httpContextAccessor.HttpContext,
            page: "/Login/reset",
            handler: null,
            values: new { token = webToken });
        var emailBody = $"Someone has requested a reset for your password on Auditor.\nIf this wasn't you, you can safely ignore this email.\n\n Reset Link: {actionURL}";
        await _emailService.SendEmailAsync(user.Email, "Auditor: Password Reset", emailBody);
    }

    public Task ResetUserPassword(string resetToken, string password)
    {
        throw new NotImplementedException();
    }

    public async Task<UserResults> SignupUser(string name, string email, string password)
    {
        if ((await _db.Users.FirstOrDefaultAsync(u => u.Email == email)) != null)
        {
            return UserResults.Error(ErrType.EXISTING_EMAIL_FOUND, "Email already has an account.");
        }
        User newUser = new()
        {
            Name = name,
            Email = email,
            PasswordHash = HashPassword(password)
        };
        await _db.Users.AddAsync(newUser);
        await _db.SaveChangesAsync();
        return UserResults.Ok(newUser);

    }
}