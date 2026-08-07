using System.ComponentModel.DataAnnotations;

namespace Auditor.Dtos;

public class UserIncoming
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;
}