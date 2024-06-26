namespace SportHub.Data.ViewModels.Users;

using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Username is required!")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required!")]
    public string Password { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is required!")]
    public string Email { get; set; }
}