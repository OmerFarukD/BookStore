using AutoMapper;
using Repositories.Abstracts;
using Services.Abstracts;

namespace Services.Concretes;

public class ServiceManager : IServiceManager
{

    private readonly IRepositoryManager _manager;

    private readonly Lazy<IBookService> _bookService;
    
    
    public ServiceManager(IRepositoryManager manager,IMapper mapper)
    {
        _manager = manager;
        
        _bookService = new Lazy<IBookService>(() => new BookManager(_manager,mapper));
    }

    public IBookService BookService => _bookService.Value;
}