namespace Entities.Exceptions;

public class CategoryNotFoundException : NotFoundException
{
    public CategoryNotFoundException(int id) : base($"Kategori bulunamadı Kategori id: {id}")
    {
    }
}