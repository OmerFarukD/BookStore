using Microsoft.AspNetCore.Mvc;
using Services.Abstracts;
namespace Presentation.Controllers;
[ApiController]
[ApiVersion("2.0",Deprecated = true)]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "v2")]
public class BooksV2Controller : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public BooksV2Controller(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet("getallBooks")]
    public async Task<IActionResult> GetAllBooksAsync()
    {
        var books = await _serviceManager.BookService.GetAllBooks(false);
        return Ok(books);
    }
}