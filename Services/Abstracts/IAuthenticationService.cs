using Entities.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Services.Abstracts;

public interface IAuthenticationService
{
    Task<IdentityResult> Register(UserForRegistrationDto userForRegistrationDto);
    Task<bool> ValidateUser(UserForLoginDto userForLoginDto);
    Task<TokenDto> CreateToken(bool populateExp);
    Task<TokenDto> RefreshToken(TokenDto tokenDto);
}