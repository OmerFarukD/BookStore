using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstracts;

namespace Repositories.EfCore;

public class CategoryRepository :RepositoryBase<Category>, ICategoryRepository
{
    public CategoryRepository(RepositoryContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges)
    {
        return await FindAll(trackChanges).OrderBy(c => c.CategoryName).ToListAsync();
    }

    public async Task<Category> GetAllCategoryByIdAsync(int id, bool trackChanges)
    {
        return await FindByCondition(x => x.CategoryId.Equals(id),trackChanges).SingleOrDefaultAsync();
    }

    public void CreateOneCategory(Category category)
    {
        Create(category);
    }
}