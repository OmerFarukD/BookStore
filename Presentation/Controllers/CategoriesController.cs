using Microsoft.AspNetCore.Mvc;
using Services.Abstracts;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public CategoriesController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllAsync()
    {
        var data = await _serviceManager.CategoryService.GetAllCategoriesAsync(false);
        return Ok(data);
    }

    [HttpGet("getBYİD")]
    public async Task<IActionResult> GetByIdAsync([FromQuery] int id)
    {
        var data = await _serviceManager.CategoryService.GetOneCategoryAsync(id, false);
        return Ok(data);
    }
}