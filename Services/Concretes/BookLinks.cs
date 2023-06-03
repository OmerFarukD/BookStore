using Entities.Dtos;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using Services.Abstracts;

namespace Services.Concretes;

public class BookLinks : IBookLinks
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IDataShaper<BookDto> _dataShaper;

    public BookLinks(LinkGenerator linkGenerator, IDataShaper<BookDto> dataShaper)
    {
        _linkGenerator = linkGenerator;
        _dataShaper = dataShaper;
    }
    public LinkResponse TryGenerateLinks(IEnumerable<BookDto> books, string fields, HttpContext httpContext)
    {
        var shapedBooks = ShapeData(books,fields);
        if (ShouldGenerateLinks(httpContext))
            return ReturnLinkedBooks(books,fields,httpContext,shapedBooks);

        return ReturnsShapedBooks(shapedBooks);
    }

    private LinkResponse ReturnLinkedBooks(IEnumerable<BookDto> books, string fields, HttpContext httpContext, List<Entity> shapedBooks)
    {
        var bookDtoList = books.ToList();
        for (int index = 0; index < bookDtoList.Count(); index++)
        {
            var bookLinks = CreateForBook(httpContext,bookDtoList[index],fields);
            
            shapedBooks[index].Add("Links",bookLinks);
        }

        var bookCollection = new LinkCollectionWrapper<Entity>(shapedBooks);
        return new LinkResponse() { Haslinks = true, LinkedEntities = bookCollection, ShappedEntities = shapedBooks };
    }

    private List<Link> CreateForBook(HttpContext httpContext, BookDto bookDto, string fields)
    {
        var links = new List<Link>()
        {
            new Link("a1","b1","c1"),
            new Link("a2","b2","c2"),
        };
        return links;
    }


    private LinkResponse ReturnsShapedBooks(List<Entity> shapedBooks)
    {
        return new LinkResponse() { ShappedEntities = shapedBooks };
    }

    private bool ShouldGenerateLinks(HttpContext httpContext)
    {
        var mediaTypes =(MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
        return mediaTypes.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
    }

    private List<Entity> ShapeData(IEnumerable<BookDto> books, string fields)
    {
      return  _dataShaper.ShapeData(books, fields).Select(b=>b.Entity).ToList();
    }
}