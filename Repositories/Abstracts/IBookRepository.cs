using Entities.Models;

namespace Repositories.Abstracts;

public interface IBookRepository : IRepositoryBase<Book>
{
    Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges);
    Task<Book?> GetAllBookByIdAsync(int id,bool trackChanges);
    void CreateOneBook(Book book);
    void DeleteOneBook(Book book);
    void UpdateOneBook(Book book);
}