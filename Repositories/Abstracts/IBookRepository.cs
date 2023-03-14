using Entities.Models;

namespace Repositories.Abstracts;

public interface IBookRepository : IRepositoryBase<Book>
{
    IQueryable<Book> GetAllBooks(bool trackChanges);
    Book? GetAllBookById(int id,bool trackChanges);
    void CreateOneBook(Book book);
    void DeleteOneBook(Book book);
    void UpdateOneBook(Book book);
}