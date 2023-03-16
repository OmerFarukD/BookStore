using AutoMapper;
using Entities.Dtos;
using Entities.Exceptions;
using Entities.Models;
using Repositories.Abstracts;
using Services.Abstracts;

namespace Services.Concretes;

public class BookManager : IBookService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerService _loggerService;
    private readonly IMapper _mapper;

    public BookManager(IRepositoryManager repositoryManager, ILoggerService loggerService, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _loggerService = loggerService;
        _mapper = mapper;
    }
    public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
    {
        return _mapper.Map<IEnumerable<BookDto>>( _repositoryManager.Book.GetAllBooks(trackChanges));
    }

    public Book GetOneBookById(int id, bool trackChanges)
    {
        var entity = _repositoryManager.Book.GetAllBookById(id,trackChanges);
        if (entity is null)
        {
            _loggerService.LogInfo($"Book with id: {id} could not found");
            throw new Exception($"Book with id: {id} could not found");
        }
        return entity;
    }

    public Book CreateOneBook(Book book)
    {
        if (book is null)
        {
            _loggerService.LogInfo("book not added");
            throw new ArgumentNullException();
        }
            
        _repositoryManager.Book.Create(book); 
        _repositoryManager.Save();
         return book;
    }



    public void UpdateOneBook(int id, BookForUpdate bookForUpdate, bool trackChanges)
    {

        var entity = _repositoryManager.Book.GetAllBookById(id, trackChanges);

        if (entity is null) throw new BookNotFound(id);

        entity = _mapper.Map<Book>(bookForUpdate);
        
        _repositoryManager.Book.Update(entity);
        _repositoryManager.Save();
    }


    public void DeleteOneBook(int id, bool trackChanges)
    {
        var entity = _repositoryManager.Book.GetAllBookById(id, trackChanges);
        if (entity is null)
        {
            _loggerService.LogInfo($"Book with id: {id} could not found");
            throw new Exception($"Book with id: {id} could not found");
        }
        _repositoryManager.Book.DeleteOneBook(entity);
        _repositoryManager.Save();
    }
}