using Entities.Dtos;
using Entities.Models;

namespace Services.Abstracts;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetAllBooks(bool trackChanges);
   Task<BookDto> GetOneBookById(int id,bool trackChanges);
    Task<BookDto> CreateOneBook(BookDtoForInsertion book);
    Task UpdateOneBook(int id, BookForUpdate bookForUpdate, bool trackChanges);
    Task DeleteOneBook(int id, bool trackChanges);
}