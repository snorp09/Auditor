using System.ComponentModel.DataAnnotations;

public class UserLogin
{
    [Required]
    public string Email = string.Empty;

    [Required]
    public string Password = string.Empty;
}