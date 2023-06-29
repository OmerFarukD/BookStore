namespace Services.Abstracts;

public interface IServiceManager
{
     IBookService BookService { get;}
     IAuthenticationService AuthenticationService { get; }
     ICategoryService CategoryService { get; }
}