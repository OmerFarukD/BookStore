using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos;

public record UserForLoginDto
{
    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    public string? UserName { get; init; }
    
    
    [Required(ErrorMessage = "Parola zorunludur.")]
    public string? Password { get; init; }
}