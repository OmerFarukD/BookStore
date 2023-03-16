using Entities.Dtos;
using Entities.Models;

namespace Services.Abstracts;

public interface IBookService
{
    IEnumerable<BookDto> GetAllBooks(bool trackChanges);
    Book GetOneBookById(int id,bool trackChanges);
    Book CreateOneBook(Book book);
    void UpdateOneBook(int id, BookForUpdate bookForUpdate, bool trackChanges);
    void DeleteOneBook(int id, bool trackChanges);
}