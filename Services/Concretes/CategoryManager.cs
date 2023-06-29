using Entities.Exceptions;
using Entities.Models;
using Repositories.Abstracts;
using Services.Abstracts;

namespace Services.Concretes;

public class CategoryManager : ICategoryService
{
    private readonly IRepositoryManager _repositoryManager;

    public CategoryManager(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges)
    {
        return await _repositoryManager.Category.GetAllCategoriesAsync(trackChanges);
    }

    public async Task<Category> GetOneCategoryAsync(int id, bool trackChanges)
    {
        var category= await _repositoryManager.Category.GetAllCategoryByIdAsync(id, trackChanges);

        if (category is null)
            throw new CategoryNotFoundException(id);

        return category;

    }
}