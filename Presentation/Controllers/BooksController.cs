
using Entities.Dtos;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Abstracts;

namespace Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public BooksController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }


    [HttpGet("getall")]
    public IActionResult GetAll()
    {
        var books = _serviceManager.BookService.GetAllBooks(false);
            return Ok(books);

    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
    {
        var book = _serviceManager.BookService.GetOneBookById(id, false);
            return Ok(book);
       
    }

    [HttpPost]
    public IActionResult CreateOneBook([FromBody] Book book)
    {
        if (book is null) return BadRequest();
            _serviceManager.BookService.CreateOneBook(book);
            return StatusCode(201, book);
        
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateOneBook([FromRoute(Name="id")]int id,[FromBody] BookForUpdate book)
    {
       
            if (book is null) return BadRequest();
            _serviceManager.BookService.UpdateOneBook(id,book,true);
            return NoContent();
        
     
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
    {
        _serviceManager.BookService.DeleteOneBook(id,true);
            return NoContent();

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