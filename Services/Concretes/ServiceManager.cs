using AutoMapper;
using Repositories.Abstracts;
using Services.Abstracts;

namespace Services.Concretes;

public class ServiceManager : IServiceManager
{
    
    private readonly Lazy<IBookService> _bookService;


    public ServiceManager(IRepositoryManager manager,IMapper mapper,IBookLinks bookLinks)
    {
       
        
        _bookService = new Lazy<IBookService>(() => new BookManager(manager,mapper,bookLinks));
    }

    public IBookService BookService => _bookService.Value;
}