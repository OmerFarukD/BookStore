using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Entities.Dtos;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Abstracts;

namespace Services.Concretes;

public class AuthenticationManager : IAuthenticationService
{
    private readonly ILoggerService _loggerService;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private User _user;

    public AuthenticationManager(ILoggerService loggerService, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
    {
        _loggerService = loggerService;
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
        _user = new User();
    }

    public async Task<IdentityResult> Register(UserForRegistrationDto userForRegistrationDto)
    {
        var user = _mapper.Map<User>(userForRegistrationDto);
        var result = await _userManager.CreateAsync(user, userForRegistrationDto.Password);
        if (result.Succeeded)
            await _userManager.AddToRolesAsync(user, userForRegistrationDto.Roles);
        return result;

    }

    public async Task<bool> ValidateUser(UserForLoginDto userForLoginDto)
    {
        _user = await _userManager.FindByNameAsync(userForLoginDto.UserName);
        var result = (_user is not null && await _userManager.CheckPasswordAsync(_user, userForLoginDto.Password));
        if (!result)
        {
            _loggerService.LogWarning($"{nameof(ValidateUser)} : Kullanıcı adı vey şifre yanlış.");
        }

        return result;
    }

    public async Task<TokenDto> CreateToken(bool populateExp)
    {
        var signingCredentials = GetCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials,claims);
        var refreshToken = GenerateRefreshToken();
        _user.RefreshToken = refreshToken;
        if (populateExp)
            _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await _userManager.UpdateAsync(_user);
        var accessToken= new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
    {
        var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken!);
            var user = await _userManager.FindByNameAsync(principal.Identity!.Name);
            
            if (user is null || user.RefreshToken != tokenDto.RefreshToken||user.RefreshTokenExpiryTime<=DateTime.Now )
            {
                throw new RefreshTokenBadRequestException();
            }
            _user = user;
            return await CreateToken(false);
            
            
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var validIssuer = jwtSettings["validIssuer"];
        var validAudience = jwtSettings["validAudience"];
        var expiration = jwtSettings["expires"];

        var tokenOptions = new JwtSecurityToken(
            issuer:validIssuer,
            audience:validAudience,
            claims:claims,
            expires:DateTime.Now.AddMinutes(Convert.ToDouble(expiration)),
            signingCredentials:signingCredentials
        );
        return tokenOptions;
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name,_user.UserName),

        };
        var roles = await _userManager.GetRolesAsync(_user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role,role));
        }

        return claims;
    }

    private SigningCredentials GetCredentials()
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey =Encoding.UTF8.GetBytes(jwtSettings["secretKey"]);
        var secret = new SymmetricSecurityKey(secretKey);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha512Signature);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng= RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey =jwtSettings["secretKey"];
        var validIssuer = jwtSettings["validIssuer"];
        var validAudience = jwtSettings["validAudience"];
        var expiration = jwtSettings["expires"];
        var tokenValidationParameters=new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = validIssuer,
            ValidAudience = validAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters,out securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken is null || jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        return principal;
    }
    
}