using AutoMapper;
using Repositories.Abstracts;
using Services.Abstracts;

namespace Services.Concretes;

public class ServiceManager : IServiceManager
{

    private readonly IRepositoryManager _manager;

    private readonly Lazy<IBookService> _bookService;
    
    
    public ServiceManager(IRepositoryManager manager,ILoggerService loggerService,IMapper mapper)
    {
        _manager = manager;
        
        _bookService = new Lazy<IBookService>(() => new BookManager(_manager,loggerService,mapper));
    }

    public IBookService BookService => _bookService.Value;
}