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
    public async Task<IEnumerable<BookDto>> GetAllBooks(bool trackChanges)
    {
        return _mapper.Map<IEnumerable<BookDto>>(await _repositoryManager.Book.GetAllBooksAsync(trackChanges));
    }

    public async Task<BookDto> GetOneBookById(int id, bool trackChanges)
    {
        var entity = await _repositoryManager.Book.GetAllBookByIdAsync(id,trackChanges);
        if (entity is null)
        {
            _loggerService.LogInfo($"Book with id: {id} could not found");
            throw new Exception($"Book with id: {id} could not found");
        }
        
        
        return _mapper.Map<BookDto>(entity);
    }

    public async Task<BookDto> CreateOneBook(BookDtoForInsertion book)
    {
        var entity = _mapper.Map<Book>(book);
        _repositoryManager.Book.CreateOneBook(entity);
        await _repositoryManager.SaveAsync();
        return _mapper.Map<BookDto>(entity);

    }


    public async Task UpdateOneBook(int id, BookForUpdate bookForUpdate, bool trackChanges)
    {

        var entity = await _repositoryManager.Book.GetAllBookByIdAsync(id, trackChanges);

        if (entity is null) throw new BookNotFound(id);

        entity = _mapper.Map<Book>(bookForUpdate);
        
        _repositoryManager.Book.Update(entity);
        await _repositoryManager.SaveAsync();
    }


    public async Task DeleteOneBook(int id, bool trackChanges)
    {
        var entity = await _repositoryManager.Book.GetAllBookByIdAsync(id, trackChanges);
        if (entity is null)
        {
            _loggerService.LogInfo($"Book with id: {id} could not found");
            throw new Exception($"Book with id: {id} could not found");
        }
        _repositoryManager.Book.DeleteOneBook(entity);
        await _repositoryManager.SaveAsync();
    }
}