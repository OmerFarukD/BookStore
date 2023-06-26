using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Repositories.Abstracts;
using Services.Abstracts;

namespace Services.Concretes;

public class ServiceManager : IServiceManager
{
    
    private readonly Lazy<IBookService> _bookService;
    private readonly Lazy<IAuthenticationService> _authenticationService;


    public ServiceManager(IRepositoryManager manager,
        IMapper mapper,
        IBookLinks bookLinks,
        UserManager<User> userManager,
        ILoggerService loggerService,
        IConfiguration configuration
        )
    {
        _authenticationService = new Lazy<IAuthenticationService>(() => 
            new AuthenticationManager(loggerService,mapper,userManager,configuration));
        _bookService = new Lazy<IBookService>(() => new BookManager(manager,mapper,bookLinks));
    }

    public IBookService BookService => _bookService.Value;
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}