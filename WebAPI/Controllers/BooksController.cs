/*using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Abstracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public BooksController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }


    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {

            var books = _serviceManager.BookService.GetAllBooks(false);
            return Ok(books);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
    {
        try
        {
            var book = _serviceManager.BookService.GetOneBookById(id, false);
            return Ok(book);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    [HttpPost]
    public IActionResult CreateOneBook([FromBody] Book book)
    {
        try
        {
            
            if (book is null) return BadRequest();
            _serviceManager.BookService.CreateOneBook(book);
            return StatusCode(201, book);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateOneBook([FromRoute(Name="id")]int id,[FromBody] Book book)
    {
        try
        {
            if (book is null) return BadRequest();
            _serviceManager.BookService.UpdateOneBook(id,book,true);
            return NoContent();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
    {
        try
        {
            _serviceManager.BookService.DeleteOneBook(id,true);
            return NoContent();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
       
    }
}*/