using Entities.Models;

namespace Services.Abstracts;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges);
    Task<Category> GetOneCategoryAsync(int id, bool trackChanges);
}