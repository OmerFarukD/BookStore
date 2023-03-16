using Entities.Models;
using Repositories.Abstracts;

namespace Repositories.EfCore;

public class BookRepository : RepositoryBase<Book>,IBookRepository
{
    public BookRepository(RepositoryContext context) : base(context)
    {
        
    }

    public IQueryable<Book> GetAllBooks(bool trackChanges) => FindAll(trackChanges).OrderBy(x=>x.Id);

    public Book? GetAllBookById(int id, bool trackChanges) => FindByCondition(x => x.Id.Equals(id), trackChanges).SingleOrDefault();

    public void CreateOneBook(Book book) => Create(book);

    public void DeleteOneBook(Book book) => Delete(book);


    public void UpdateOneBook(Book book) => Update(book);
}