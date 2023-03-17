namespace Repositories.Abstracts;

public interface IRepositoryManager
{
    public IBookRepository Book { get;}
    Task SaveAsync();
}