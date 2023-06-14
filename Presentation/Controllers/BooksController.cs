
using System.Text.Json;
using Entities.Dtos;
using Entities.RequestFeatures;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Abstracts;

namespace Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(LogFilterAttribute),Order = 2)]
[HttpCacheExpiration(CacheLocation = CacheLocation.Public,MaxAge = 80)]
public class BooksController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public BooksController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }


    [HttpHead]
    [HttpGet("getall",Name = "GetAll")]
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
  //  [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetAll([FromQuery]BookParameters bookParameters)
    {

        var linkParameters = new LinkParameters()
        {
            BookParameters = bookParameters,
            HttpContext = HttpContext
        };
        var result =await _serviceManager.BookService.GetAllBooks(linkParameters, false);
        Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(result.metaData));
            return result.linkResponse.Haslinks ? Ok(result.linkResponse.LinkedEntities):Ok(result.linkResponse.ShappedEntities);

    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOneBook([FromRoute(Name = "id")] int id)
    {
        var book = await _serviceManager.BookService.GetOneBookById(id, false);
            return Ok(book);
       
    }

    [HttpPost("createonebook",Name = "CreateOneBook")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
  
    public async Task<IActionResult> CreateOneBook([FromBody] BookDtoForInsertion book)
    {
        if (book is null) return BadRequest();
        
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
             await _serviceManager.BookService.CreateOneBook(book);
            return StatusCode(201, book);
        
    }

    [HttpPut("{id:int}")]
    [ServiceFilter(typeof(ValidationFilterAttribute),Order = 1)]
   
    public async Task<IActionResult> UpdateOneBook([FromRoute(Name="id")]int id,[FromBody] BookForUpdate book)
    {
       
            if (book is null) return BadRequest();
            
          
            
            await _serviceManager.BookService.UpdateOneBook(id,book,false);
            return NoContent();
        
     
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOneBook([FromRoute(Name = "id")] int id)
    {
        await _serviceManager.BookService.DeleteOneBook(id,true);
            return NoContent();

    }

    [HttpOptions]
    public IActionResult GetBooksOptions()
    {
        Response.Headers.Add("Allow","GET, PUT, POST, PATCH, DELETE, HEAD, OPTIONS");
        return Ok();
    }

    /*[HttpPatch("{id:int}")]
    public IActionResult PartiallyUpdateOneBook([FromRoute(Name="id") ]int id, [FromBody] JsonPatchDocument<BookForUpdate> patchBook)
    {
        var entity = _serviceManager.BookService.GetOneBookById(id,true);
            if (entity is null) throw new BookNotFound(id);
            
            patchBook.ApplyTo(entity);
            _serviceManager.BookService.UpdateOneBook(id,entity,true);

            return NoContent();
    }*/
}