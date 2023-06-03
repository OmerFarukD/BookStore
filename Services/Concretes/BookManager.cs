using System.Dynamic;
using AutoMapper;
using Entities.Dtos;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Abstracts;
using Services.Abstracts;

namespace Services.Concretes;
public class BookManager : IBookService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IDataShaper<BookDto> _dataShaper;
    
    public BookManager(IRepositoryManager repositoryManager, IMapper mapper,IDataShaper<BookDto> dataShaper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _dataShaper = dataShaper;
    }
    public async Task<(IEnumerable<ShapedEntity> books, MetaData metaData)> GetAllBooks(BookParameters bookParameters,
        bool trackChanges)
    {
        if (!bookParameters.ValidPriceRange)
        {
            throw new PriceOutOfRangeException();
        }
        
        
        var booksWithMetaData = await _repositoryManager.Book.GetAllBooksAsync(bookParameters, trackChanges);
        var booksDto = _mapper.Map<IEnumerable<BookDto>>(booksWithMetaData);
        var shapedData = _dataShaper.ShapeData(booksDto,bookParameters.Fields);
        return (shapedData,booksWithMetaData.MetaData);
    }

    public async Task<BookDto> GetOneBookById(int id, bool trackChanges)
    {
        var entity = await _repositoryManager.Book.GetAllBookByIdAsync(id,trackChanges);
        if (entity is null)
        {
            throw new BookNotFound(id);
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
            throw new BookNotFound(id);
        }
        _repositoryManager.Book.DeleteOneBook(entity);
        await _repositoryManager.SaveAsync();
    }
}