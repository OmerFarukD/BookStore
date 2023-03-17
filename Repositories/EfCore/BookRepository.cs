using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstracts;

namespace Repositories.EfCore;

public class BookRepository : RepositoryBase<Book>,IBookRepository
{
    public BookRepository(RepositoryContext context) : base(context)
    {
        
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges) =>await FindAll(trackChanges).OrderBy(x=>x.Id).ToListAsync();

    public async Task<Book?> GetAllBookByIdAsync(int id, bool trackChanges) =>await FindByCondition(x => x.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

    public void CreateOneBook(Book book) => Create(book);

    public void DeleteOneBook(Book book) => Delete(book);


    public void UpdateOneBook(Book book) => Update(book);
}