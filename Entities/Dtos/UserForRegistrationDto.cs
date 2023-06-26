using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos;

public record UserForRegistrationDto
{
    public string? FirstName { get; init; }   
    public string? LastName { get; init; }   
    
    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    public string? UserName { get; init; }   
    
    [Required(ErrorMessage = "Parola  zorunludur.")]
    public string? Password { get; init; }   
    public string? Email { get; init; }   
    public string? PhoneNumber { get; init; }
    public ICollection<string>? Roles { get; init; }
    
}