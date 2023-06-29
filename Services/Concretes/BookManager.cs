using System.Dynamic;
using AutoMapper;
using Entities.Dtos;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Abstracts;
using Services.Abstracts;

namespace Services.Concretes;
public sealed class BookManager : IBookService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IBookLinks _bookLinks;

    public BookManager(IRepositoryManager repositoryManager, IMapper mapper, IBookLinks bookLinks)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _bookLinks = bookLinks;
    }

    public async Task<(LinkResponse linkResponse , MetaData metaData)> GetAllBooks(LinkParameters linkParameters,
        bool trackChanges)
    {
        if (!linkParameters.BookParameters.ValidPriceRange)
        {
            throw new PriceOutOfRangeException();
        }
        
        
        var booksWithMetaData = await _repositoryManager.Book.GetAllBooksAsync(linkParameters.BookParameters, trackChanges);
        var booksDto = _mapper.Map<IEnumerable<BookDto>>(booksWithMetaData);
        var links = _bookLinks.TryGenerateLinks(booksDto, linkParameters.BookParameters.Fields,
            linkParameters.HttpContext);
        return (links,booksWithMetaData.MetaData);
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

    public async Task<List<BookDto>> GetAllBooks(bool trackChanges)
    {
        var books = await _repositoryManager.Book.GetAllBooksAsync(trackChanges);
        var data = _mapper.Map<List<BookDto>>(books);
        return data;
    }

    public async Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges)
    {
        return await _repositoryManager.Book.GetAllBooksWithDetailsAsync(trackChanges);
    }
}