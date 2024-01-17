using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models;

public class RegisterModel{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 6)]
    public string Password { get; set; }    

    [Required]
    [StringLength(50, MinimumLength = 6)]
    public string ConfirmPassword { get; set; } 
}