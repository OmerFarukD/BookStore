using Entities.Models;
using Entities.RequestFeatures;

namespace Repositories.Abstracts;

public interface IBookRepository : IRepositoryBase<Book>
{
    Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters,bool trackChanges);

    Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges);
    Task<Book?> GetAllBookByIdAsync(int id,bool trackChanges);
    void CreateOneBook(Book book);
    void DeleteOneBook(Book book);
    void UpdateOneBook(Book book);

    Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges);
}