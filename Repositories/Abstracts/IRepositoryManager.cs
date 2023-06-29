namespace Repositories.Abstracts;

public interface IRepositoryManager
{
    public IBookRepository Book { get;}
    public ICategoryRepository Category { get;}
    Task SaveAsync();
}