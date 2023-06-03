using System.Dynamic;
using Entities.Dtos;
using Entities.Models;
using Entities.RequestFeatures;

namespace Services.Abstracts;

public interface IBookService
{
    Task<(IEnumerable<ShapedEntity> books, MetaData metaData)> GetAllBooks(BookParameters bookParameters,
        bool trackChanges);
   Task<BookDto> GetOneBookById(int id,bool trackChanges);
    Task<BookDto> CreateOneBook(BookDtoForInsertion book);
    Task UpdateOneBook(int id, BookForUpdate bookForUpdate, bool trackChanges);
    Task DeleteOneBook(int id, bool trackChanges);
}