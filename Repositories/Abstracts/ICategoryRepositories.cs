using Entities.Models;

namespace Repositories.Abstracts;

public interface ICategoryRepository : IRepositoryBase<Category>
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges);
    Task<Category> GetAllCategoryByIdAsync(int id,bool trackChanges);

    void CreateOneCategory(Category category);
}