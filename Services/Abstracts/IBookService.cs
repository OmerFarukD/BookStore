using System.Dynamic;
using Entities.Dtos;
using Entities.LinkModels;
using Entities.Models;
using Entities.RequestFeatures;

namespace Services.Abstracts;

public interface IBookService
{
    Task<(LinkResponse linkResponse, MetaData metaData)> GetAllBooks(LinkParameters linkParameters,
        bool trackChanges);
   Task<BookDto> GetOneBookById(int id,bool trackChanges);
    Task<BookDto> CreateOneBook(BookDtoForInsertion book);
    Task UpdateOneBook(int id, BookForUpdate bookForUpdate, bool trackChanges);
    Task DeleteOneBook(int id, bool trackChanges);
    Task<List<BookDto>> GetAllBooks(bool trackChanges);
}