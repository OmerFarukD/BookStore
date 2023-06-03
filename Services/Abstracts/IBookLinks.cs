using Entities.Dtos;
using Entities.LinkModels;
using Microsoft.AspNetCore.Http;

namespace Services.Abstracts;

public interface IBookLinks
{
    LinkResponse TryGenerateLinks(IEnumerable<BookDto> books,string fields, HttpContext httpContext);
}