
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
    public async Task<IActionResult> GetAll()
    {
        var books =await _serviceManager.BookService.GetAllBooks(false);
            return Ok(books);

    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOneBook([FromRoute(Name = "id")] int id)
    {
        var book = await _serviceManager.BookService.GetOneBookById(id, false);
            return Ok(book);
       
    }

    [HttpPost]
    public async Task<IActionResult> CreateOneBook([FromBody] BookDtoForInsertion book)
    {
        if (book is null) return BadRequest();
        
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
             await _serviceManager.BookService.CreateOneBook(book);
            return StatusCode(201, book);
        
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateOneBook([FromRoute(Name="id")]int id,[FromBody] BookForUpdate book)
    {
       
            if (book is null) return BadRequest();
            
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
            
            await _serviceManager.BookService.UpdateOneBook(id,book,false);
            return NoContent();
        
     
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOneBook([FromRoute(Name = "id")] int id)
    {
        await _serviceManager.BookService.DeleteOneBook(id,true);
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