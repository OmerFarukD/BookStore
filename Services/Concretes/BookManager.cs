using Entities.Models;
using Repositories.Abstracts;
using Services.Abstracts;

namespace Services.Concretes;

public class BookManager : IBookService
{
    private readonly IRepositoryManager _repositoryManager;

    public BookManager(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    public IEnumerable<Book> GetAllBooks(bool trackChanges)
    {
        return _repositoryManager.Book.GetAllBooks(trackChanges);
    }

    public Book GetOneBookById(int id, bool trackChanges)
    {
        var entity = _repositoryManager.Book.GetAllBookById(id,trackChanges);
        if (entity is null) throw new Exception($"Book with id: {id} could not found");
        return entity;
    }

    public Book CreateOneBook(Book book)
    {
        if (book is null)
            throw new ArgumentNullException();
        _repositoryManager.Book.Create(book); 
        _repositoryManager.Save();
         return book;
    }

    public void UpdateOneBook(int id, Book book, bool trackChanges)
    {
        var entity = _repositoryManager.Book.GetAllBookById(id, trackChanges);

        if (entity is null) throw new Exception($"Book with id: {id} could not found");
        if (book is null) throw new ArgumentNullException(nameof(book));
        
        _repositoryManager.Book.Update(book);
        _repositoryManager.Save();
    }


    public void DeleteOneBook(int id, bool trackChanges)
    {
        var entity = _repositoryManager.Book.GetAllBookById(id, trackChanges);
        if (entity is null)
            throw new Exception($"Book with id: {id} could not found");
        _repositoryManager.Book.DeleteOneBook(entity);
        _repositoryManager.Save();
    }
}